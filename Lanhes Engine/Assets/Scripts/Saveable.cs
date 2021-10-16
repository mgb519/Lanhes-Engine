using Newtonsoft.Json.Linq;
public interface ISaveable
{
    //Do not like how I have to pass around doc just so that I can call CreateElement
    JObject SaveToFile();


    void LoadFromFile(JObject node);
}

