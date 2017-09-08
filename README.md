# MonTask #

## What is it? ##
A library which extends `System.Task` with fluent syntax using LINQ convention.

## Example ## 
### Map Generic Tasks ###
```csharp
using System;
using System.Task;
using MonTask;

namespace Example {
  public static class Program {

    public static void Main(string[] args) {
      // Returns the lenght of the string given to method ExampleAsync
      Task<int> select = ExampleAsync("Hello World").Select(str => str.Length);
    }

    public static Task<string> ExampleAsync(string txt) {
      return Task.Run(() => txt);
    }
  }
}
```

### Flatten Generic Tasks ###
```csharp
using System;
using System.Task;
using MonTask;

namespace Example {
  public static class Program {

    public static void Main(string[] args) {
      // Using map with a task will nest the task.
      Task<Task<string>> select = ExampleAsync("Hello World").Select(str => ExampleAsync(str));
      // Using flatmap will not nest the task.
      Task<string> selectMany = ExampleAsync("Hello World").SelectMany(str => ExampleAsync(str));
    }

    public static Task<string> ExampleAsync(string txt) {
      return Task.Run(() => txt);
    }
  }
}
```
### Flatten Void Tasks ###
```csharp
using System;
using System.Task;
using MonTask;

namespace Example {
  public static class Program {

    public static void Main(string[] args) {
      // Using map with a task will nest the task.
      Task<Task> select = ExampleAsync().Select(str => ExampleAsync());
      // Using flatmap will not nest the task.
      Task selectMany = ExampleAsync().SelectMany(str => ExampleAsync());
    }

    public static Task ExampleAsync() {
      return Task.Delay(1000);
    }
  }
}
```

### Install: [NuGet Package Manager](https://www.nuget.org/packages/MonTask/) ###
