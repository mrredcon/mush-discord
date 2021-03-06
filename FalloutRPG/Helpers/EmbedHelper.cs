﻿using Discord;

namespace FalloutRPG.Helpers
{
    public class EmbedHelper
    {
        /// <summary>
        /// Builds a simple Embed with a title and content.
        /// </summary>
        /// <remarks>
        /// The embed description has a max of 2048 characters.
        /// </remarks>
        public static Embed BuildBasicEmbed(string title, string content)
        {
            content = StringHelper.Truncate(content, 2048);

            var builder = new EmbedBuilder()
                .WithDescription(content)
                .WithColor(new Color(0, 128, 255))
                .WithAuthor(author => {
                    author
                        .WithName(title);
                });

            return builder.Build();
        }

        /// <summary>
        /// Builds a simple Embed title, content and fields.
        /// </summary>
        /// <remarks>
        /// The embed description has a max of 2048 characters. 
        /// fieldTitles and fieldContents arrays must be the same
        /// length.
        /// </remarks>
        public static Embed BuildBasicEmbedWithFields(string title, string content, string[] fieldTitles, string[] fieldContents)
        {
            if (fieldTitles.Length != fieldContents.Length)
                return null;

            var builder = new EmbedBuilder()
                .WithDescription(content)
                .WithColor(new Color(0, 128, 255))
                .WithAuthor(author => {
                    author
                        .WithName(title);
                });

            for (var i = 0; i < fieldTitles.Length; i++)
            {
                builder.AddField(fieldTitles[i], fieldContents[i]);
            }

            return builder.Build();
        }
    }
}
