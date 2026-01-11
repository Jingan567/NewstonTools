# System.Text.Json和Newtonsoft.Json

✅ 首选：**`System.Text.Json`**（.NET Core 3.0+ 内置）

- **协议**：MIT（微软官方，完全免费）
- **优点**：
  - 性能极高（比 Newtonsoft.Json 快 20%~50%）
  - 内存占用低
  - 支持异步流式解析（`Utf8JsonReader`/`Writer`）
  - 默认安全（不支持循环引用，防 DoS）
- **缺点**：API 略严格（如不自动处理 `DateTime` 格式）
- **适用**：新项目、高性能服务、ASP.NET Core 默认

```C#
var json = JsonSerializer.Serialize(obj);
var obj = JsonSerializer.Deserialize<T>(json);
```

⚠️ 旧项目兼容：**`Newtonsoft.Json`（Json.NET）**

- **协议**：MIT（免费商用）
- **优点**：功能最全、社区生态庞大、支持复杂场景（循环引用、自定义转换器）
- **缺点**：性能略低于 `System.Text.Json`
- **适用**：遗留系统、需要高级功能（如 `JsonProperty` 灵活映射）

> 🔗 官网：https://www.newtonsoft.com/json

## System.Text.Json的用法

