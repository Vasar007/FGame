using System;
using System.IO;
using Acolyte.Assertions;
using FGame.Models;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class ActorViewModel
    {
        private readonly Actors.Actor _actor;

        // Subtract 1 since data's indexes start at 1 instead of 0.
        public int PosX => (_actor.Pos.X - 1) * 10;

        public int PosY => (_actor.Pos.Y - 1) * 10;

        public string Text => Actors.getChar(_actor).ToString();

        public string? ImagePath => FindImagePath();


        public ActorViewModel(Actors.Actor actor)
        {
            _actor = actor.ThrowIfNull(nameof(actor));
        }

        private string? FindImagePath()
        {
            string assemblyName = typeof(ActorViewModel).Assembly.GetName().Name
                ?? throw new InvalidOperationException("Cannnot get assembly name.");

            // https://stackoverflow.com/a/2416464/8581036
            string imageUriSourceFirstPart = $"/{assemblyName};component/";

            const string resourceDirName = "Resources";
            string? imageName = TryGetImageName();

            return imageName is null
                ? null
                : imageUriSourceFirstPart + Path.Combine(resourceDirName, imageName);
        }

        private string? TryGetImageName()
        {
            if (_actor.Kind.Equals(Actors.ActorKind.Acorn))
            {
                return "Acorn.png";
            }

            if (_actor.Kind.Equals(Actors.ActorKind.Doggo))
            {
                return "Doggo.png";
            }

            if (_actor.Kind.Equals(Actors.ActorKind.Rabbit))
            {
                return "Rabbit.png";
            }

            if (_actor.Kind.Equals(Actors.ActorKind.Tree))
            {
                return "Tree.png";
            }

            if (_actor.Kind.Equals(Actors.ActorKind.NewSquirrel(true)))
            {
                return "SquirrelAcorn.png";
            }

            if (_actor.Kind.Equals(Actors.ActorKind.NewSquirrel(false)))
            {
                return "Squirrel.png";
            }

            return null;
        }
    }
}
