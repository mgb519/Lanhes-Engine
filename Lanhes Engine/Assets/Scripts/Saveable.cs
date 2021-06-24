
using System.Xml;
public interface ISaveable
{
    //Do not like how I have to pass around doc just so that I can call CreateElement
    XmlNode SaveToFile(XmlDocument doc);


    void LoadFromFile(XmlNode node);
}

