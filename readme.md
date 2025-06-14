本项目旨在解决飞书文档的批量导出。
- 本项目是基于https://github.com/xhnbzdl/feishu-doc-export 改造而成，感谢作者的付出，解决了我的实际问题；
- 本项目全部是用AI来生成，感谢vscode的ai编程助手；
- 本项目是基于.net9+Avalonia做的跨端客户程序，本人只在mac电脑上测试过，其他环境，需要执行dotnet相关的命令。感谢.net 9 + Avalonia,让我这个10年前从c#转到java的开发者，能再用上c#

## 如何使用

### 获取AppId和AppSecret

- 进入飞书[开发者后台](https://open.feishu.cn/app)，创建企业自建应用，信息随意填写。进入应用的后台管理页
- （重要）打开权限管理，开通需要的权限：云文档>开通以下权限（注意有分页）
  - 查看新版文档
  - 查看、评论和下载云空间中所有文件
  - 查看、评论和导出文档
  - 查看、评论、编辑和管理云空间中所有文件
  - 查看、评论、编辑和管理多维表格
  - 查看、编辑和管理知识库
  - 查看、评论、编辑和管理电子表格
  - 导出云文档
- 打开添加应用能力，添加机器人
- 版本管理与发布中创建一个版本，并申请发布上线
  - 等待企业管理员审核通过
  - 如果只是为了测试，可以选择测试企业和人员，创建测试企业，绑定应用，切换至测试版本
    - 进入测试企业创建知识库和文档
- 为机器人添加知识库的访问权限，具体步骤如下：
  - 在飞书桌面客户端中创建一个新的群组或直接使用已有的群组
  - 为群组添加群机器人，选择上面步骤中自己创建的应用作为群机器人
  - 打开知识库，如果你是知识库管理员，则可以看见知识空间设置。打开知识空间设置>成员管理>添加管理员，选择刚刚建立的群组
- 回到开发者平台，打开凭证与基础信息，获取 `App ID` 和 `App Secret`

### 如何获取知识库ID
![image](https://github.com/xhnbzdl/feishu-doc-export/assets/84184815/ba45e7c8-ff76-4591-bda1-366f6234a6d0)
![image](https://github.com/xhnbzdl/feishu-doc-export/assets/84184815/8be655df-9168-4c2a-90d6-81dff8e1676a)

### 编译执行
下载.net 9,下载源码，然后执行即可。
 ```powershell
dotnet build
dotnet run
 ```
<img width="503" alt="image" src="https://github.com/user-attachments/assets/3401541b-1550-4570-b029-d3c1822e44c1" />
