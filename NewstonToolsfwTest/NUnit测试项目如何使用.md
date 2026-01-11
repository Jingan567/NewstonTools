在 NUnit 项目中进行单元测试，涉及**创建测试项目、编写测试类与方法、使用断言、运行测试**等核心步骤。下面为你提供一份 **完整、实用、符合最佳实践的 NUnit 测试指南**（适用于 .NET 6+ / .NET Core / .NET Framework）。

------

## 🧪 一、准备工作：创建 NUnit 测试项目

### ✅ 方法 1：使用 .NET CLI（推荐）

bash

```
# 创建解决方案（可选）
dotnet new sln -n MySolution

# 创建被测项目（例如类库）
dotnet new classlib -n MyLibrary
dotnet sln add MyLibrary

# 创建 NUnit 测试项目
dotnet new nunit -n MyLibrary.Tests
dotnet sln add MyLibrary.Tests

# 添加项目引用
dotnet add MyLibrary.Tests reference MyLibrary
```

### ✅ 方法 2：使用 Visual Studio

- 文件 → 新建 → 项目 → 搜索 “NUnit”
- 选择 **“NUnit Test Project”**
- 安装后自动包含必要 NuGet 包

------

## 📦 二、必需的 NuGet 包（通常已自动安装）

| 包                       | 作用                                      |
| ------------------------ | ----------------------------------------- |
| `NUnit`                  | 核心测试框架                              |
| `NUnit3TestAdapter`      | 让 Visual Studio / Rider 能发现并运行测试 |
| `Microsoft.NET.Test.Sdk` | .NET 测试 SDK（必须）                     |

> 💡 如果手动创建，需显式安装：
>
> bash
>
> ```
> dotnet add package NUnit
> dotnet add package NUnit3TestAdapter
> dotnet add package Microsoft.NET.Test.Sdk
> ```

------

## 🧱 三、编写第一个 NUnit 测试

### 被测代码（`MyLibrary/Calculator.cs`）

csharp

```
namespace MyLibrary;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public double Divide(double dividend, double divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Divisor cannot be zero.");
        return dividend / divisor;
    }
}
```

### 测试代码（`MyLibrary.Tests/CalculatorTests.cs`）

csharp

```
using NUnit.Framework;
using MyLibrary;

namespace MyLibrary.Tests;

[TestFixture] // ← 标记这是一个测试类
public class CalculatorTests
{
    private Calculator _calculator = null!; // 非空初始化（C# 8+ 可空上下文）

    [SetUp] // ← 每个测试方法执行前运行
    public void Setup()
    {
        _calculator = new Calculator();
    }

    [Test] // ← 标记这是一个测试方法
    public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange（准备）
        int a = 2, b = 3;
        int expected = 5;

        // Act（执行）
        int result = _calculator.Add(a, b);

        // Assert（断言）
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Assert.Throws 验证异常
        Assert.Throws<DivideByZeroException>(() =>
            _calculator.Divide(10, 0));
    }

    // 参数化测试：用 TestCase 提供多组输入
    [TestCase(10, 2, ExpectedResult = 5.0)]
    [TestCase(9, 3, ExpectedResult = 3.0)]
    [TestCase(-6, 2, ExpectedResult = -3.0)]
    public double Divide_ValidInputs_ReturnsCorrectQuotient(double dividend, double divisor)
    {
        return _calculator.Divide(dividend, divisor);
    }
}
```

------

## 🔑 四、NUnit 核心特性详解

### 1. **测试类标记**

- `[TestFixture]`：标记测试类（可省略，但建议保留以提高可读性）

### 2. **测试方法标记**

- `[Test]`：普通测试方法
- `[TestCase(...)]`：参数化测试（一行代码测多组数据）
- `[Theory]` + `[Datapoint]`：更复杂的参数化（较少用）

### 3. **生命周期方法**

| 属性                | 执行时机           | 用途                       |
| ------------------- | ------------------ | -------------------------- |
| `[OneTimeSetUp]`    | 整个测试类开始前   | 初始化数据库连接等昂贵资源 |
| `[SetUp]`           | **每个测试方法前** | 创建被测对象（SUT）        |
| `[TearDown]`        | 每个测试方法后     | 清理临时文件、重置状态     |
| `[OneTimeTearDown]` | 整个测试类结束后   | 关闭连接、释放全局资源     |

> ✅ **最佳实践**：
>
> - 用 `[SetUp]` 初始化被测对象（保证每个测试独立）
> - 避免测试间共享状态！

------

### 4. **断言（Assertions）—— NUnit 的灵魂**

#### 基础断言

csharp

编辑

```
Assert.AreEqual(expected, actual);
Assert.IsTrue(condition);
Assert.IsNotNull(obj);
```

#### ✅ **推荐：使用 Constraint-Based 断言（更清晰）**

csharp

编辑

```
Assert.That(result, Is.EqualTo(5));
Assert.That(list, Has.Count.EqualTo(3));
Assert.That(text, Does.StartWith("Hello"));
Assert.That(exception, Throws.TypeOf<ArgumentNullException>());
```

#### 常用约束

| 约束                      | 示例            |
| ------------------------- | --------------- |
| `Is.EqualTo(x)`           | 值相等          |
| `Is.SameAs(x)`            | 引用相同        |
| `Has.Length`, `Has.Count` | 集合/字符串长度 |
| `Does.Contain(x)`         | 包含元素        |
| `Throws.Exception`        | 抛出任意异常    |
| `Is.Null`, `Is.Not.Null`  | 空值检查        |

------

### 5. **忽略或条件测试**

csharp

编辑

```
[Ignore("暂时跳过，因为...")]
[Test]
public void TemporarilyDisabledTest() { }

[Explicit] // 需手动运行（不在 CI 中自动执行）
[Test]
public void ManualOnlyTest() { }

// 条件编译
[Test, Platform("Win")] // 仅 Windows 运行
public void WindowsOnlyTest() { }
```

------

## ▶️ 五、运行 NUnit 测试

### ✅ 方法 1：命令行（CI/CD 推荐）

bash

编辑

```
dotnet test
# 或指定项目
dotnet test MyLibrary.Tests/
```

### ✅ 方法 2：Visual Studio

- 测试 → Windows → 测试资源管理器
- 右键测试类/方法 → “运行测试”

### ✅ 方法 3：Rider / VS Code

- 安装 NUnit 插件后，直接点击 ⚡ 图标运行

------

## 🧪 六、高级场景示例

### 场景 1：测试异步方法

csharp

```
[Test]
public async Task GetDataAsync_ValidId_ReturnsData()
{
    var service = new DataService();
    var result = await service.GetDataAsync(123);
    Assert.That(result.Id, Is.EqualTo(123));
}
```

### 场景 2：Mock 依赖（配合 Moq）

csharp

```
// 安装 Moq: dotnet add package Moq
[Test]
public void ProcessOrder_WhenInventoryLow_ThrowsException()
{
    var mockRepo = new Mock<IInventoryRepository>();
    mockRepo.Setup(r => r.GetStock("A1")).Returns(0);
    
    var service = new OrderService(mockRepo.Object);
    
    Assert.Throws<InsufficientStockException>(() => 
        service.ProcessOrder("A1", 10));
}
```

### 场景 3：测试私有方法？→ **不要测！**

- 单元测试应只测 **公共行为**
- 如果必须测内部逻辑，改用 `[InternalsVisibleTo]` 暴露 internal 成员：

csharp

```
// 在 MyLibrary/AssemblyInfo.cs（或 .csproj）
[assembly: InternalsVisibleTo("MyLibrary.Tests")]
```

然后将方法改为 `internal`，而非 `private`。

------

## 🧹 七、最佳实践总结

| 原则                 | 说明                                                         |
| -------------------- | ------------------------------------------------------------ |
| **AAA 模式**         | Arrange-Act-Assert 结构清晰                                  |
| **一个测试一个职责** | 测试方法名描述行为（如 `Add_NegativeAndPositive_ReturnsCorrectSum`） |
| **不依赖外部系统**   | 数据库、网络 → 用 Mock/Fake 替代                             |
| **快速 & 独立**      | 测试应在毫秒级完成，互不影响                                 |
| **覆盖边界条件**     | 0、null、负数、最大值等                                      |
| **命名规范**         | `<Method>_<Scenario>_<ExpectedBehavior>`                     |

------

## 📂 八、典型项目结构

text

编辑

```
MySolution/
├── MyLibrary/                # 被测项目
│   ├── Calculator.cs
│   └── ...
└── MyLibrary.Tests/          # 测试项目
    ├── CalculatorTests.cs
    ├── OrderServiceTests.cs
    └── TestHelpers/          # 共享工具（可选）
        └── MockFactory.cs
```

------

## 🔚 总结

- **NUnit = `[TestFixture]` + `[Test]` + `Assert.That()`**
- **每个测试必须独立、快速、可重复**
- **优先使用 Constraint-Based 断言**（`Is.EqualTo` 而非 `AreEqual`）
- **用 `[SetUp]` 初始化，避免状态污染**
- **永远不要测 `private` 方法** → 重构为 `internal` + `[InternalsVisibleTo]` 或通过公共 API 间接测试

> 💡 **记住**：
>  **好的单元测试 = 文档 + 安全网 + 设计反馈**

现在你已经掌握了 NUnit 的核心！如果需要 **Moq 集成示例**、**测试覆盖率报告（coverlet）** 或 **NUnit 与 xUnit 对比**，欢迎继续提问！