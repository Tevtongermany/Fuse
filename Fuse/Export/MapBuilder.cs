using System;
using System.Collections.Generic;
using System.Text;

namespace Fuse.Export;

public class MapBuilder
{
    private StringBuilder _builder = new();

    private int _indentLevel = 0;

    private const string INDENTATION = "   ";

    public void Property(string name, string value)
    {
        Write($"{name}={value}");
    }

    public void Write(string data)
    {
        for (var i = 0; i < _indentLevel; i++)
        {
            _builder.Append(INDENTATION);
        }

        _builder.Append(data);
        _builder.Append('\n');
    }

    public void Section(string identifier, Action inner)
    {
        Section(identifier, [], inner);
    }
    
    public void Section(string identifier, List<HMapProperty> properties, Action inner)
    {
        var propertiesBuilder = new StringBuilder();
        foreach (var property in properties)
        {
            propertiesBuilder.Append(property.Name);
            propertiesBuilder.Append('=');
            propertiesBuilder.Append(property.Value);
            propertiesBuilder.Append(' ');
        }
        
        Write($"Begin {identifier} {propertiesBuilder}");
        _indentLevel++;
        
        inner.Invoke();
        
        _indentLevel--;
        Write($"End {identifier}");
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}

public record HMapProperty(string Name, string Value);