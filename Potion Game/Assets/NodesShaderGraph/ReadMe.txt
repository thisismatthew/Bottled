When using ParallaxMapping or CreateNormalChannel, be sure to set Sampler 
State otherwise there will be errors

For the Parallax occlusion Mapping node, 
the variable must be correctly set to "UseChanel_1_4_RGBA",
 the number 1 - this means that the node will use the Red channel 
 from the texture, 
 the number 2 - means that it will use the Green channel, 
 3 - Blue, 4 - Alpha.

To use nodes for amplify shader editor, import files from the "NodesForAmplifyShaderEditor" file.


If you are using a version of unity lower than 2019.3,
  remove this package, remove the package in the
  same way - C:\Users\Name\AppData\Roaming\Unity\Asset Store-5.x\Andrey Graphics\Shaders\Nodes for Shader Graph.unitypackage

After that, re-download the package from the asset store for
 the Unity 2019 else 2018 version, in the new version of asset store, the wrong version is downloaded.


!!! If the material becomes pink or does not work properly, open the shader
in the Shader Graph and click Save, the problem disappears


 In HDRP, for SSS use the Diffusion Profile in your HDRenderPipelineAsset 

 If you have Shader Graph 6.9.0 update it