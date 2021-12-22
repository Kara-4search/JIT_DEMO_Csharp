# JIT_DEMO_Csharp

Blog link: not gonna update

- A Demo to replace method with JIT(Or we could called "creating managed hooks")
- The DEBUG block in Project.cs line 121 is for debuging.
- Abount getting function address is kind of different betweent debuging or compiling

## Usage
- Just a Demo
	1.The picture down below is the running result.
	- The format of the Relative Address below is wrong,but it been fixed.
	![avatar](https://raw.githubusercontent.com/Kara-4search/ProjectPics/main/JIT_Demo.png)
## TO-DO list
- None

## Update history
- Fixed bug: Relative Address output in wrong format - 20211222

## Reference link:
	1. https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.compilerservices.runtimehelpers.preparemethod?view=net-6.0
	2. https://gist.github.com/HoLLy-HaCKeR/c1fcee3df1ea35e7816c55147e6db78d
	3. https://www.codenong.com/cs106378408/
	4. http://www.ostack.cn/?qa=1418702/
	5. https://www.buzzphp.com/posts/dynamically-append-code-to-a-method-net-core