namespace Wjt.FilmWorld.Payloads;

public record MovieItem(
    string Title,
    string Year,
    string ID,
    string Type,
    string Poster
);