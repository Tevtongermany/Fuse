using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using Serilog;


namespace Fuse.Export;

public static class HMapExporter
{
    public static string Export(UWorld world)
    {
        var level = world.PersistentLevel.Load<ULevel>();
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
                        classPath = template.GetPathName().Replace("Default__", string.Empty).Replace("FortniteGame/Content/","/Game/");
                    }

                    var staticMeshComponent = actor.GetOrDefault<UStaticMeshComponent?>("StaticMeshComponent");
                    List<String> Hurensohn = ["S_Elevation_Ground_Z_39", "S_Elevation_Ground_Z14236", "S_Elevation_Ground_Z14234", "S_Elevation_Ground_Z_58"];

                    if (classPath.StartsWith("/Script/FortniteGame.FortStaticMeshActor")) // If its just a simple static mesh instead of a actor
                    {
                        if (staticMeshComponent is null) return;
                        var meshactor = staticMeshComponent.GetOrDefault<UStaticMesh>("StaticMesh");
                        var overrideMaterial = staticMeshComponent.GetOrDefault<List<UMaterialInstanceConstant>>("OverrideMaterials");

                        if (meshactor is null) continue;

                        foreach (var hund in Hurensohn)
                        {
                            if (hund == actor.Name)
                            {
                                Log.Information("{0}", actor.Name);
                                Log.Information("{0}", staticMeshComponent.GetOrDefault("RelativeLocation", FVector.OneVector).ToHMap());
                                Log.Information("{0}", staticMeshComponent.GetOrDefault("RelativeRotation", FRotator.ZeroRotator).ToHMap());
                                Log.Information("{0}", staticMeshComponent.GetOrDefault("RelativeScale3D", FVector.OneVector).ToHMap());
                            }
                        }

                        Log.Information(actor.Name);
                        Log.Information("Creating Actor Section");
                        builder.Section("Actor", [new HMapProperty("Class", classPath), new HMapProperty("Name", actor.Name),], () =>
                        {
                            Log.Information("Creating object Section");
                            builder.Section("Object", [new HMapProperty("Name", "StaticMeshComponent0")], () => // Static Mesh Component
                            {
                                Log.Information("Creating Static Mesh Section");
                                builder.Property("StaticMesh", $"\"/Script/Engine.StaticMesh\'{meshactor.GetPathName().Replace("FortniteGame/Content/", "/Game/")}\'\"");
                                Log.Information("Checking if has override material");
                                if (overrideMaterial is not null)
                                {
                                    Log.Information("has override material adding it");
                                    int i = 0;
                                    foreach (var material in overrideMaterial)
                                    {
                                        if (material is null) return;
                                        builder.Property($"OverrideMaterials({i})",material.GetPathName().Replace("FortniteGame/Content/", "/Game/"));
                                        i++;
                                    }
                                }

                                Log.Information("Adding Location rotation scale data");
                                builder.Property("RelativeLocation", staticMeshComponent.GetOrDefault("RelativeLocation",FVector.OneVector).ToHMap());
                                builder.Property("RelativeRotation", staticMeshComponent.GetOrDefault("RelativeRotation", FRotator.ZeroRotator).ToHMap());
                                builder.Property("RelativeScale3D", staticMeshComponent.GetOrDefault("RelativeScale3D", FVector.OneVector).ToHMap());
                            });
                            Log.Information("adding data");
                            builder.Property("ActorLabel", $"\"{actor.Name}\"");
                            builder.Property("RootComponent", "StaticMeshComponent0");
                            builder.Property("StaticMeshComponent", "StaticMeshComponent0");
                            Log.Information("");
                        });

                        continue;
                    }
                    // Actor
                    builder.Section("Actor", [new HMapProperty("Class", classPath), new HMapProperty("Name", actor.Name),], () =>
                    {
                        builder.Property("ActorLabel", $"\"{actor.Name}\"");
                        if (staticMeshComponent is null) return;
                        var getmirrored = actor.GetOrDefault("bMirrored",false);

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
                                if (staticMeshComponent.TryGetValue(out RelativeRotation, "RelativeRotation"))
                                {
                                    builder.Property("RelativeRotation", RelativeRotation.ToHMap());
                                }
                                if (staticMeshComponent.TryGetValue(out RelativeScale3D, "RelativeScale3D"))
                                {
                                    builder.Property("RelativeScale3D", RelativeScale3D.ToHMap());
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
                            if (staticMeshComponent.TryGetValue(out RelativeRotation, "RelativeRotation"))
                            {
                                builder.Property("RelativeRotation", RelativeRotation.ToHMap());
                            }
                            if (staticMeshComponent.TryGetValue(out RelativeScale3D, "RelativeScale3D"))
                            {
                                builder.Property("RelativeScale3D", RelativeScale3D.ToHMap());
                            }

                        });
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
        return $"(X={vector.X.ToString().Replace(',', '.'):0.0000000},Y={vector.Y.ToString().Replace(',', '.'):0.0000000},Z={vector.Z.ToString().Replace(',', '.'):0.0000000})";
    }

    public static string ToHMap(this FRotator rotator)
    {
        return $"(Pitch={rotator.Pitch.ToString().Replace(',', '.'):0.0000000},Yaw={rotator.Yaw.ToString().Replace(',', '.'):0.0000000},Roll={rotator.Roll.ToString().Replace(',', '.'):0.0000000})";
    }
}

