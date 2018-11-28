# MagnetX
找磁力链接、找种子的小工具。它具备以下特点：</br>
  <ul>√异步、多线程地实时爬取多个网站。</ul>
  <ul>√可以非明文地缓存历史搜索结果，在本地历史纪录中更快速地搜索关键字。</ul>
  <ul>√绿色天然无公害，远离弹窗，远离广告。</ul>

# Example
<img src="https://raw.githubusercontent.com/chanket/MagnetX/master/MagnetX.png" width="901" height="507" />

# Customize
MagnetX提供了一个不错的框架，如果你有兴趣也可以实现自己的数据源，自给自足，丰衣足食。</br>
只要在程序集下的类同时具有以下3个特点，你定义的源的实现即可通过C#的Reflection机制直接被识别：</br>
1.继承自`MagnetX.Searcher.Searcher`类</br>
2.类具有`MagnetX.Searcher.SearcherEnabledAttribute`属性</br>
3.具有无参数构造函数</br>
## 继承MagnetX.Searcher.SimpleWebSearcher类
对于网页爬虫源，最快捷的实现方式为继承`MagnetX.Searcher.SimpleWebSearcher`类，并给出解析源的关键实现。
```csharp
[SearcherEnabled]                             //标记源为启用的属性
class FooWebSearcher : SimpleWebSearcher      //通过SimpleWebSearcher间接继承Searcher类
{
    //必须覆盖的方法
    //定义word关键词第i页结果所在的URL
    protected override async Task<string> GetURLAsync(string word, int page)
    {
        string name = Uri.EscapeUriString(word);
        return "https://www.foo.biz/s/" + name + "_rel_" + page + ".html";
    }

    //必须实现的无参数构造函数
    //通过传递给基类5个参数快速实现一个网页爬虫源。
    public CilibaWebSearcher()
        : base("foo.biz",                       //源的友好名称。
              "<div class=\"search-item\">",    //每一个搜索结果的前导特征字符串。基类会对结果用这个字符串进行切割，并处理从下标1开始的字串。
              new Regex("target=\"_blank\">(.+?)</a>", RegexOptions.Compiled),          //资源名称的正则表达式。
              new Regex("<a href=\".+?/detail/(.+?)\\.html", RegexOptions.Compiled),    //资源哈希值的正则表达式。
              new Regex("文件大小.+?>(.+?)<", RegexOptions.Compiled))                    //资源文件大小的正则表达式。
    {

    }

    //可选的覆盖方法
    //在这里可以对基类得到的结果进行进一步处理，例如去除一些标签。
    //有时正则表达式无法直接得到最终结果（例如，需要参考资源详情页面才能获得资源哈希值），
    //也可以在这个方法里做进一步处理。
    protected override async Task<Result> GetResultAsync(string part)
    {
        Result result = await base.GetResultAsync(part).ConfigureAwait(false);  //获得基类的得到的中间结果
        if (result == null) return null;                                        //注意基类结果发生错误时结果为null

        result.Name = result.Name.Replace("<em>", "").Replace("</em>", "");
        return result;
    }
}
```
