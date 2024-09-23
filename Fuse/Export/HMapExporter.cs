using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using Serilog;
using SharpGLTF.Schema2;


namespace Fuse.Export;

public static class HMapExporter
{
    public static string Export(UWorld world)
    {
        if (world.PersistentLevel.Load<ULevel>() is not { } level)
            return "";
        var actors = level?.Actors ?? [];

        var builder = new MapBuilder();

        builder.Section("Map", () =>
        {
            builder.Section("Level", () =>
            {
                foreach (var lazyActor in actors)
                {
                    if (lazyActor is null || lazyActor.IsNull) continue;

                    var actor = lazyActor.Load();
                    if (actor is null) continue;

                    var className = actor.Class?.Name ?? string.Empty;
                    var source = className.StartsWith("Fort") ? "FortniteGame" : "Engine";
                    var classPath = $"/Script/{source}.{className}";
                    if (actor.Template?.Load() is { } template)
                    {
                        classPath = template.GetPathName().Replace("Default__", string.Empty).Replace("FortniteGame/Content/", "/Game/");
                    }

                    var staticMeshComponent = actor.GetOrDefault<UStaticMeshComponent?>("StaticMeshComponent");



                    if (classPath.StartsWith("/Script/FortniteGame.FortStaticMeshActor")) // If its just a simple static mesh instead of a actor
                    {
                        if (staticMeshComponent is null) continue;
                        var meshactor = staticMeshComponent.GetOrDefault<UStaticMesh>("StaticMesh");
                        var overrideMaterial = staticMeshComponent.GetOrDefault<List<UMaterialInstanceConstant>>("OverrideMaterials");

                        if (meshactor is null) continue;
                        
                        builder.Section("Actor", [new HMapProperty("Class", classPath), new HMapProperty("Name", actor.Name),], () =>
                        {
                            builder.Section("Object", [new HMapProperty("Name", "StaticMeshComponent0")], () => // Static Mesh Component
                            {
                                builder.Property("StaticMesh", $"\"/Script/Engine.StaticMesh\'{meshactor.GetPathName().Replace("FortniteGame/Content/", "/Game/")}\'\"");
                                if (overrideMaterial is not null)
                                {
                                    int i = 0;
                                    foreach (var material in overrideMaterial)
                                    {
                                        builder.Property($"OverrideMaterials({i})", material.GetPathName().Replace("FortniteGame/Content/", "/Game/"));
                                        i++;
                                    }
                                }

                                FVector RelativeLocation;
                                FRotator RelativeRotation;
                                FVector RelativeScale3D;

                                if (staticMeshComponent.TryGetValue(out RelativeLocation, "RelativeLocation"))
                                {
                                    builder.Property("RelativeLocation", RelativeLocation.ToHMap());

                                }
                                else
                                {
                                    builder.Property("RelativeLocation", staticMeshComponent.GetOrDefault("RelativeLocation", FVector.ZeroVector).ToHMap());
                                }

                                if (staticMeshComponent.TryGetValue(out RelativeRotation, "RelativeRotation"))
                                {
                                    builder.Property("RelativeRotation", RelativeRotation.ToHMap());

                                }
                                else
                                {
                                    builder.Property("RelativeRotation", staticMeshComponent.GetOrDefault("RelativeRotation", FRotator.ZeroRotator).ToHMap());
                                }

                                if (staticMeshComponent.TryGetValue(out RelativeScale3D, "RelativeScale3D"))
                                {
                                    builder.Property("RelativeScale3D", RelativeScale3D.ToHMap());

                                }
                                else
                                {
                                    builder.Property("RelativeScale3D", staticMeshComponent.GetOrDefault("RelativeScale3d", FVector.OneVector).ToHMap());
                                }
                            });
                            builder.Property("ActorLabel", $"\"{actor.Name}\"");
                            builder.Property("RootComponent", "StaticMeshComponent0");
                            builder.Property("StaticMeshComponent", "StaticMeshComponent0");

                        });

                        if (actor.Name.Equals("S_Elevation_Ground_Z_58", StringComparison.OrdinalIgnoreCase))
                        {
                            Log.Information("Hs");
                        }

                        continue;
                    }
                    // Actor
                    builder.Section("Actor", [new HMapProperty("Class", classPath), new HMapProperty("Name", actor.Name),], () =>
                    {
                        builder.Property("ActorLabel", $"\"{actor.Name}\"");
                        if (staticMeshComponent is null) return;
                        var getmirrored = actor.GetOrDefault("bMirrored", false);

                        

                        if (getmirrored)
                        {
                            builder.Section("Object", [new HMapProperty("Name", staticMeshComponent.Name)], () =>
                            {
                                FVector RelativeLocation;
                                FRotator RelativeRotation;
                                FVector RelativeScale3D;

                                if (staticMeshComponent.TryGetValue(out RelativeLocation, "RelativeLocation"))
                                {
                                    builder.Property("RelativeLocation", RelativeLocation.ToHMap());
                                }
                                else
                                {
                                    builder.Property("RelativeLocation", staticMeshComponent.GetOrDefault("RelativeLocation", FVector.OneVector).ToHMap());
                                }

                                if (staticMeshComponent.TryGetValue(out RelativeRotation, "RelativeRotation"))
                                {
                                    builder.Property("RelativeRotation", RelativeRotation.ToHMap());
                                }
                                else
                                {
                                    builder.Property("RelativeRotation", staticMeshComponent.GetOrDefault("RelativeRotation", FVector.OneVector).ToHMap());
                                }

                                if (staticMeshComponent.TryGetValue(out RelativeScale3D, "RelativeScale3D"))
                                {
                                    builder.Property("RelativeScale3D", RelativeScale3D.ToHMap());
                                }
                                else
                                {
                                    builder.Property("RelativeScale3D", staticMeshComponent.GetOrDefault("RelativeScale3d", FVector.OneVector).ToHMap());
                                }

                            });
                            builder.Property("bMirrored", getmirrored.ToString());
                            return;

                        }



                        builder.Section("Object", [new HMapProperty("Name", staticMeshComponent.Name)], () =>
                        {
                            FVector RelativeLocation;
                            FRotator RelativeRotation;
                            FVector RelativeScale3D;
                            if (staticMeshComponent.TryGetValue(out RelativeLocation, "RelativeLocation"))
                            {
                                builder.Property("RelativeLocation", RelativeLocation.ToHMap());
                            }
                            else
                            {
                                builder.Property("RelativeLocation", staticMeshComponent.GetOrDefault("RelativeLocation", FVector.OneVector).ToHMap());
                            }
                            if (staticMeshComponent.TryGetValue(out RelativeRotation, "RelativeRotation"))
                            {
                                builder.Property("RelativeRotation", RelativeRotation.ToHMap());
                            }
                            else
                            {
                                builder.Property("RelativeRotation", staticMeshComponent.GetOrDefault("RelativeRotation", FVector.OneVector).ToHMap());
                            }
                            if (staticMeshComponent.TryGetValue(out RelativeScale3D, "RelativeScale3D"))
                            {
                                builder.Property("RelativeScale3D", RelativeScale3D.ToHMap());
                            }
                            else
                            {
                                builder.Property("RelativeScale3D", staticMeshComponent.GetOrDefault("RelativeScale3d", FVector.OneVector).ToHMap());
                            }
                        });



                        UObject[] textureDataRawPaths;
                        if (actor.TryGetAllValues<UObject>(out textureDataRawPaths, "TextureData"))
                        {
                            if (actor.Name == "BrickSimple_RoofWall2")
                            {
                                Log.Information("");
                            }
                            int i = 0;
                            foreach (var texture in textureDataRawPaths)
                            {
                                if (texture is null)
                                {
                                    i++;
                                    continue;

                                }
                                builder.Property($"TextureData({i})", $"\"/Script/FortniteGame.BuildingTextureData\'{texture.GetPathName().Replace("FortniteGame/Content/", "/Game/").Replace("/Diffuses/","/")}\'\"");
                                i++;

                            }
                            Log.Information("{0}", $" ");
                        }
                    });
                }
            });
        });


        return builder.ToString();
    }
}

public static class HMapExporterExtensions
{
    public static string ToHMap(this FVector vector)
    {
        return $"(X={vector.X.ToString().Replace(',', '.')},Y={vector.Y.ToString().Replace(',', '.')},Z={vector.Z.ToString().Replace(',', '.')})";
    }

    public static string ToHMap(this FRotator rotator)
    {
        return $"(Pitch={rotator.Pitch.ToString().Replace(',', '.')},Yaw={rotator.Yaw.ToString().Replace(',', '.')},Roll={rotator.Roll.ToString().Replace(',', '.')})";
    }
}

