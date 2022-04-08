

using System;
using UnityEngine.Localization.Metadata;

[Serializable]
[Metadata(AllowedTypes = MetadataType.Locale)]
public class IsRTL : IMetadata
{
    public bool isRTL = false;
}
