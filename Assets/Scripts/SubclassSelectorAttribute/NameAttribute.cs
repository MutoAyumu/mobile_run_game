using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class NameAttribute : Attribute
{
    public string Name { get; }
    public NameAttribute(Type name) => Name = name.ToString();
}
