using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "MonoDevelopRPC",
    Namespace = "MonoDevelopRPC",
    Version = "0.4"
)]

[assembly: AddinName("MonoDevelopRPC")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Adds Discord Rich Presence")]
[assembly: AddinAuthor("NepNet")]

[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]