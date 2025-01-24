namespace Contract.BusinessRules.Constants;

public sealed class PreferenceConstants
{
    public class Preferences
    {
        public sealed class WhatYouWant
        {
            public Guid Id = new("eba69204-6653-47af-b913-cda8efb387b0");

            public readonly string[] Selections =
            [
                "🌙 Reduce stress, anxiety, and enhance emotional well-being",
                "💆 Access self-help tools, guided meditations, and mindfulness exercises",
                "🏋️‍♂️ Connect with like-minded individuals, share progress and seek advice",
                "🧘 Boost physical health and increase energy levels",
                "💆 Receive personalized recommendations, enhance skills and achieve personal goals",
                "🍎 Establish a healthy routine and develop positive habits",
                "🧠 Improve focus, memory, and mental clarity"
            ];

            public const string Title = "✨ What you want us to help you?✨";
        }

        public sealed class YourMental
        {

        }

        public sealed class YourHobby
        {

        }
    }
}
