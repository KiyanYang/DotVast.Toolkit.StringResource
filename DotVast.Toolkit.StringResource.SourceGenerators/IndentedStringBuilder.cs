using System;
using System.Text;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

internal sealed class IndentedStringBuilder
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly string _tabString;
    private int _indentLevel;
    private bool _tabsPending = true;

    public IndentedStringBuilder(string tabString)
    {
        _tabString = tabString;
    }

    public int Indent
    {
        get => _indentLevel;
        set => _indentLevel = Math.Max(value, 0);
    }

    public IndentedStringBuilder Append(string value)
    {
        OutputTabs();
        _stringBuilder.Append(value);
        return this;
    }

    public IndentedStringBuilder AppendLine(string value)
    {
        OutputTabs();
        _stringBuilder.AppendLine(value);
        _tabsPending = true;
        return this;
    }

    public IndentedStringBuilder AppendFormat(string format, object arg)
    {
        OutputTabs();
        _stringBuilder.AppendFormat(format, arg);
        return this;
    }

    public IndentedStringBuilder AppendFormat(string format, params object[] args)
    {
        OutputTabs();
        _stringBuilder.AppendFormat(format, args);
        return this;
    }

    public override string ToString() => _stringBuilder.ToString();

    private void OutputTabs()
    {
        if (_tabsPending)
        {
            for (var i = 0; i < _indentLevel; i++)
            {
                _stringBuilder.Append(_tabString);
            }
            _tabsPending = false;
        }
    }
}
