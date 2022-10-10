using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;

namespace Pokégen.PkHex.Util;

public class ThreadPoolTaskScheduler : TaskScheduler
{
	private SmartThreadPool ThreadPool { get; }

	private List<Task> Tasks { get; } = new();

	// The maximum concurrency level allowed by this scheduler.
	private int MaxDegreeOfParallelism { get; }

	public ThreadPoolTaskScheduler(int maxThreads)
	{
		MaxDegreeOfParallelism = maxThreads;
		ThreadPool = new SmartThreadPool(10, MaxDegreeOfParallelism);
	}

	// Gets the maximum concurrency level supported by this scheduler.
	public sealed override int MaximumConcurrencyLevel 
		=> MaxDegreeOfParallelism;
	
	protected sealed override bool TryDequeue(Task task)
	{
		lock (Tasks) return Tasks.Remove(task);
	}
	
	protected override IEnumerable<Task>? GetScheduledTasks()
	{
		var lockTaken = false;
		try
		{
			Monitor.TryEnter(Tasks, ref lockTaken);
			if (lockTaken) return Tasks;
			else throw new NotSupportedException();
		}
		finally
		{
			if (lockTaken) Monitor.Exit(Tasks);
		}
	}

	protected override void QueueTask(Task task)
	{
		lock (Tasks)
		{
			Tasks.Add(task);
			ThreadPool.QueueWorkItem(_ =>
			{
				lock (Tasks)
				{
					Tasks.Remove(task);
				}
				
				TryExecuteTask(task);

				return null;
			});
		}
	}
	
	protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return false;
	}
}
