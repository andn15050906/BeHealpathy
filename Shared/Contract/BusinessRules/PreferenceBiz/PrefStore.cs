namespace Contract.BusinessRules.PreferenceBiz;

public sealed class PrefStore
{
    private static readonly List<Guid> _ids
        = [
            new("439c920c-aa97-4b0c-ad15-39449b2c4ed7"),
            new("a9f1666e-a967-4d76-a662-c7f0698f5d4f"),
            new("7cd97208-0afb-4f22-a1f5-0bb3449bf129"),
            new("92ca6e85-0da4-42f5-a01c-e4922c9f9d3f"),
            new("73340458-7b3a-4575-8e79-a405699f7a35"),
            new("f4e14ff2-8a2c-4afd-bc0e-468847e5b8c6"),
            new("e0f718af-90c5-48c9-95bb-67913c0f9340"),
            new("02b1324a-cacd-47df-a12d-bbf711eea1d2"),
            new("cfbe77a4-a37e-4ea4-92cb-0bdea8bc9e8a"),
            new("0f7a1532-b6c1-4b27-b422-f1fec121623c"),
            new("0bc040ba-4035-45ed-965b-a36db4363773"),
            new("d1a71204-90da-40c9-a104-7ae6b7f5dfda"),
            new("e367cb57-cb4f-4bd4-97d7-98ff7da13b50"),
            new("59ef4967-b1ab-4d77-95b8-da02aa7c895e"),
            new("03d881c1-340d-4dc9-81e7-58ee9beb3191"),
            new("7bd6cd76-7eea-482c-992b-b69866e809b1"),
            new("62c40ff4-d7ea-438c-b43b-33289604eead"),
            new("c66da2f4-6463-41b7-b2e0-868bad7f210a"),
            new("281e828c-c90e-4b8a-8648-887ea78aa603"),
            new("63313e29-20d0-4dcf-8e4e-364afda197de")
        ];

    public static List<PreferenceSurvey> Surveys
        =>
        [
            new(
                _ids[0],
                "✨ What you want us to help you?✨",
                new() {
                    { _ids[1], "🌙 Giảm căng thẳng, lo âu và tăng cường sức khỏe cảm xúc" },
                    { _ids[2], "💆 Truy cập các công cụ tự trợ giúp, thiền có hướng dẫn và các bài tập chánh niệm" },
                    { _ids[3], "🏋️‍ Kết nối với những người có cùng chí hướng, chia sẻ tiến trình và tìm kiếm lời khuyên" },
                    { _ids[4], "🧘 Tăng cường sức khỏe thể chất và tăng mức năng lượng" },
                    { _ids[5], "💆 Nhận các đề xuất được cá nhân hóa, nâng cao kỹ năng và đạt được mục tiêu cá nhân" },
                    { _ids[6], "🍎 Thiết lập thói quen lành mạnh và phát triển những thói quen tích cực" },
                    { _ids[7], "🧠 Cải thiện sự tập trung, trí nhớ và sự minh mẫn về tinh thần" },
                }
            )
        ];
}