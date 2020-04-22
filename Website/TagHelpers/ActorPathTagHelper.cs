using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Octogami.SixDegreesOfNetflix.Application.Feature.GetPathBetweenActors;

namespace Octogami.SixDegreesOfNetflix.Website.TagHelpers
{
    public class ActorPathTagHelper : TagHelper
    {
        public ActorPath ActorPath { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Console.WriteLine("In process");

            if (ActorPath == null)
            {
                output.Content.SetHtmlContent($"<h3> There was no path found :( </h3>");
                return;
            }

            var stringBuilder = new StringBuilder();
            var nextInPath = ActorPath;

            while (nextInPath != null)
            {
                if (nextInPath.ActedIn?.With != null)
                {
                    stringBuilder.Append(
                        $"<h3>{nextInPath.Name} acted in {nextInPath.ActedIn.Title} with {nextInPath.ActedIn.With.Name}</h3>");
                }

                nextInPath = nextInPath.ActedIn?.With;
            }

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}