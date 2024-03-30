
# 1. SwitchRoutineExecutor

SysBot.Base/Control/SwitchRoutineExecutor.cs

## 1.1 ReadUntilChanged

```csharp

    /// <summary>
    /// Reads an offset until it changes to either match or differ from the comparison value.
    /// </summary>
    /// <returns>If <see cref="match"/> is set to true, then the function returns true when the offset matches the given value.<br>Otherwise, it returns true when the offset no longer matches the given value.</br></returns>
    public async Task<bool> ReadUntilChanged(ulong offset, byte[] comparison, int waitms, int waitInterval, bool match, bool absolute, CancellationToken token)
    {
        var sw = new Stopwatch();
        sw.Start();
        do
        {
            var task = absolute
                ? SwitchConnection.ReadBytesAbsoluteAsync(offset, comparison.Length, token)
                : SwitchConnection.ReadBytesAsync((uint)offset, comparison.Length, token);
            var result = await task.ConfigureAwait(false);
            if (match == result.SequenceEqual(comparison))
                return true;

            await Task.Delay(waitInterval, token).ConfigureAwait(false);
        } while (sw.ElapsedMilliseconds < waitms);
        return false;
    }
```

> /explain

The provided C# method, `ReadUntilChanged`, is an asynchronous method that continuously reads data from a specific
memory offset until the read data either matches or differs from a provided comparison value, depending on the
parameters given.

The method takes six parameters:

- `ulong offset`: This is the memory offset from where the data will be read.
- `byte[] comparison`: This is the byte array that the read data will be compared to.
- `int waitms`: This is the total time in milliseconds that the method will keep trying to read and compare the data
  before it gives up and returns false.
- `int waitInterval`: This is the time in milliseconds that the method will wait between each read attempt.
- `bool match`: This is a flag that determines whether the method should return true when the read data matches the
  comparison value (if set to true) or when the read data does not match the comparison value (if set to false).
- `bool absolute`: This is a flag that determines whether the method should read the data from an absolute memory
  offset (if set to true) or a relative memory offset (if set to false).
- `CancellationToken token`: This is a cancellation token that can be used to cancel the operation.

The method starts by creating a `Stopwatch` instance and starting it. This is used to keep track of the total time spent
in the loop.

```csharp
var sw = new Stopwatch();
sw.Start();
```

Then, it enters a loop that continues until the total elapsed time exceeds the `waitms` parameter. Inside the loop, it
reads data from the specified memory offset using either the `ReadBytesAbsoluteAsync` or `ReadBytesAsync` method of
the `SwitchConnection` object, depending on the `absolute` parameter.

```csharp
var task = absolute
    ? SwitchConnection.ReadBytesAbsoluteAsync(offset, comparison.Length, token)
    : SwitchConnection.ReadBytesAsync((uint)offset, comparison.Length, token);
var result = await task.ConfigureAwait(false);
```

After reading the data, it compares the read data to the `comparison` byte array. If the `match` parameter is true and
the read data matches the comparison data, or if the `match` parameter is false and the read data does not match the
comparison data, the method immediately returns true.

```csharp
if (match == result.SequenceEqual(comparison))
    return true;
```

If the read data does not meet the desired condition, the method waits for the time specified by the `waitInterval`
parameter before starting the next iteration of the loop.

```csharp
await Task.Delay(waitInterval, token).ConfigureAwait(false);
```

If the method exits the loop without returning true, it means that the read data did not meet the desired condition
within the specified time, so the method returns false.




