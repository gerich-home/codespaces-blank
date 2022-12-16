namespace Engine;

public record class SceneSetup(
    IShape scene,
    IShape diffuse,
    IShape glossy,
    ILightSource lights
);
