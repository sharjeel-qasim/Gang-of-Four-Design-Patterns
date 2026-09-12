using System.Text;

namespace GoFDesignPatterns.Behavioral;

// Visitor Interface
public interface IDocumentVisitor
{
    void Visit(HeadingElement heading);
    void Visit(ParagraphElement paragraph);
    void Visit(CodeBlockElement codeBlock);
    string GetOutput();
}

// Element Interface
public interface IDocumentElement
{
    void Accept(IDocumentVisitor visitor);
}

// Concrete Elements
public class HeadingElement(int level, string text) : IDocumentElement
{
    public int Level { get; } = level;
    public string Text { get; } = text;

    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

public class ParagraphElement(string text) : IDocumentElement
{
    public string Text { get; } = text;

    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

public class CodeBlockElement(string language, string code) : IDocumentElement
{
    public string Language { get; } = language;
    public string Code { get; } = code;

    public void Accept(IDocumentVisitor visitor) => visitor.Visit(this);
}

// Concrete Visitor 1: Markdown Exporter
public class MarkdownExportVisitor : IDocumentVisitor
{
    private readonly StringBuilder _builder = new();

    public void Visit(HeadingElement heading) =>
        _builder.AppendLine($"{new string('#', heading.Level)} {heading.Text}\n");

    public void Visit(ParagraphElement paragraph) =>
        _builder.AppendLine($"{paragraph.Text}\n");

    public void Visit(CodeBlockElement codeBlock) =>
        _builder.AppendLine($"```{codeBlock.Language}\n{codeBlock.Code}\n```\n");

    public string GetOutput() => _builder.ToString().TrimEnd();
}

// Concrete Visitor 2: HTML Exporter
public class HtmlExportVisitor : IDocumentVisitor
{
    private readonly StringBuilder _builder = new();

    public void Visit(HeadingElement heading) =>
        _builder.AppendLine($"<h{heading.Level}>{heading.Text}</h{heading.Level}>");

    public void Visit(ParagraphElement paragraph) =>
        _builder.AppendLine($"<p>{paragraph.Text}</p>");

    public void Visit(CodeBlockElement codeBlock) =>
        _builder.AppendLine($"<pre><code class=\"language-{codeBlock.Language}\">{codeBlock.Code}</code></pre>");

    public string GetOutput() => _builder.ToString().TrimEnd();
}

// Document Composite holding the elements
public class TechnicalDocument
{
    private readonly List<IDocumentElement> _elements = [];

    public void AddElement(IDocumentElement element) => _elements.Add(element);

    public string ExportWith(IDocumentVisitor visitor)
    {
        foreach (var element in _elements)
        {
            element.Accept(visitor);
        }
        return visitor.GetOutput();
    }
}
