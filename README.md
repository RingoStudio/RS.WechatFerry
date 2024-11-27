# WechatFerry

## 描述
### 本项目是 @github/lich0821 大佬的[WechatFerry](https://github.com/lich0821/WeChatFerry) nng接口 C#实现

### 目前实现以下功能：
- 获取通讯录
- 获取所有群成员基础信息(含wxid，群昵称，微信昵称）
- 发送文本、图片、文件、xml文章、名片、群艾特消息
- 根据wxid查询好友信息
- 根据群ID获取所有群成员WXID / 同时获取一个微信群内所有成员的群昵称
- 接收各类消息
- 群管理
- 自动合并短的文本、艾特信息（可设定单条信息最大长度）
- 自动分割过长的单条信息

## 依赖版本
WechatFerry v39.2.4 - 微信3.9.10.2 
[下载地址](https://github.com/lich0821/WeChatFerry/releases)

## 编译环境
Visual Studio 2022 + .Net 8.0

## 初始化一个机器人实例

```c#
     var wechat = new(OnWechatMessageReceived);
     wechat.Init();
```
## 更新记录
#### 2024.11.27
- 首次发布

## 免责声明
代码仅供交流学习使用，请勿用于非法用途和商业用途！如因此产生任何法律纠纷，均与作者无关！
