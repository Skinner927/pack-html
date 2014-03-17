using HtmlAgilityPack;

namespace pack_html.packers
{
    public interface IPacker
    {
        HtmlDocument Pack(HtmlDocument html, string currentDir);
    }
}