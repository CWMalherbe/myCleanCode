using Markdig;

namespace Presentation.Shared.Components;

/// <summary>
/// Just a readme
/// </summary>
public partial class ReadmeComponent
{
    /// <summary>
    /// Renders the markdown
    /// </summary>
    public string RenderMarkdown
    {
        get
        {
            var pipeline = new MarkdownPipelineBuilder().UseGridTables().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(File.ReadAllText("../README.md"), pipeline);
        }
    }
}
