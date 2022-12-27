namespace MyAttributes;

public static class MyAttributesInfo
{
    public static bool Exist(Type attributeType)
    {
        return attributeType == typeof(AfterAttribute) || attributeType == typeof(BeforeAttribute)
            || attributeType == typeof(AfterClassAttribute) || attributeType == typeof(BeforeClassAttribute);
    }
}
