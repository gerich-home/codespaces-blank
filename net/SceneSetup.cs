namespace Engine;

public record class SceneSetup(
    IBody scene,
    IBody diffuse,
    IBody glossy,
    ILightSource lights
);
