namespace MappaMentis.Domain.Aggregates;

/// <summary>
/// Root aggregate for Pomodoro Timer functionality.
/// Encapsulates all business logic for the pomodoro technique without UI dependencies.
/// </summary>
public class PomodoroTimer
{
    public Guid Id { get; private set; }
    public Guid MindMapId { get; private set; }
    public int WorkDurationMinutes { get; private set; }
    public int BreakDurationMinutes { get; private set; }
    public int SessionCount { get; private set; }
    public TimerState CurrentState { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? PausedAt { get; private set; }
    public TimeSpan? RemainingTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public PomodoroTimer(
        Guid id,
        Guid mindMapId,
        int workDurationMinutes = 25,
        int breakDurationMinutes = 5)
    {
        Id = id;
        MindMapId = mindMapId;
        WorkDurationMinutes = workDurationMinutes > 0 ? workDurationMinutes : throw new ArgumentException("Work duration must be greater than 0.", nameof(workDurationMinutes));
        BreakDurationMinutes = breakDurationMinutes > 0 ? breakDurationMinutes : throw new ArgumentException("Break duration must be greater than 0.", nameof(breakDurationMinutes));
        SessionCount = 0;
        CurrentState = TimerState.Idle;
        StartedAt = null;
        PausedAt = null;
        RemainingTime = null;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Starts the pomodoro timer.
    /// </summary>
    public void Start()
    {
        if (CurrentState == TimerState.Running)
            throw new InvalidOperationException("Timer is already running.");

        CurrentState = TimerState.Running;
        StartedAt = DateTime.UtcNow;
        PausedAt = null;
        RemainingTime = TimeSpan.FromMinutes(WorkDurationMinutes);
        SessionCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Pauses the running timer.
    /// </summary>
    public void Pause()
    {
        if (CurrentState != TimerState.Running)
            throw new InvalidOperationException("Timer is not running.");

        CurrentState = TimerState.Paused;
        PausedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    public void Resume()
    {
        if (CurrentState != TimerState.Paused)
            throw new InvalidOperationException("Timer is not paused.");

        CurrentState = TimerState.Running;
        if (PausedAt.HasValue && StartedAt.HasValue)
        {
            var pausedDuration = PausedAt.Value - StartedAt.Value;
            StartedAt = DateTime.UtcNow - pausedDuration;
        }
        PausedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Stops the timer and resets it to idle state.
    /// </summary>
    public void Stop()
    {
        CurrentState = TimerState.Idle;
        StartedAt = null;
        PausedAt = null;
        RemainingTime = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Completes the work session and transitions to break.
    /// </summary>
    public void CompleteWorkSession()
    {
        if (CurrentState != TimerState.Running && CurrentState != TimerState.Paused)
            throw new InvalidOperationException("No work session is currently active.");

        CurrentState = TimerState.OnBreak;
        RemainingTime = TimeSpan.FromMinutes(BreakDurationMinutes);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Completes the break and transitions back to idle.
    /// </summary>
    public void CompleteBreak()
    {
        if (CurrentState != TimerState.OnBreak)
            throw new InvalidOperationException("No break is currently active.");

        CurrentState = TimerState.Idle;
        RemainingTime = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the work and break durations.
    /// </summary>
    public void UpdateDurations(int workDurationMinutes, int breakDurationMinutes)
    {
        if (CurrentState != TimerState.Idle)
            throw new InvalidOperationException("Cannot change durations while timer is running or on break.");

        WorkDurationMinutes = workDurationMinutes > 0 ? workDurationMinutes : throw new ArgumentException("Work duration must be greater than 0.", nameof(workDurationMinutes));
        BreakDurationMinutes = breakDurationMinutes > 0 ? breakDurationMinutes : throw new ArgumentException("Break duration must be greater than 0.", nameof(breakDurationMinutes));
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Resets the session counter.
    /// </summary>
    public void ResetSessionCount()
    {
        SessionCount = 0;
        UpdatedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Enumeration of possible timer states.
/// </summary>
public enum TimerState
{
    Idle = 0,
    Running = 1,
    Paused = 2,
    OnBreak = 3,
    Completed = 4
}
