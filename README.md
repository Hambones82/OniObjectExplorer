This is a mod for Oxygen Not Included that allows you to browse through and modify objects at runtime.  The post build script automatically moves assets and the compiled .dll to the mods/dev/ObjectExplorer folder.

The inspectors allow you to view various members of components of gameobjects.  
This mod supports viewing "basic type" members at depth 0 or basic type members at depth 1 that are members of non-basic type memebers at depth 0.  
In other words, a member of a component that is a primitive type (e.g., int, bool, string, float), can be viewed.  
In addition, a member of a non-basic field or property that is, itself, a member of a component, can be viewed, as long as the member of the non-basic field or property is a primitive type.  
In the code, a member at depth 0 is called a member and a member at depth 1 is called a sub-member.

The object explorer allows for customization of the specific members of components that are viewable, as well as the specific sub-members that are viewable.  
Specifically, the file InspectorSpecifications.cs includes "memberspecifications" and "componentspecifications" dictionaries that let you specify the members of components and members, by name, that are to be viewed, as well as the order in which those members are displayed.  

For example, in the code already provided, Vector2 types display x and y values in that order.  

RectTransform types specify the values shown in that order.

I'm open to implementing reasonable features as suggested by people, as well as making changes to the code that make sense.