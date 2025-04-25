namespace Wjt.CinemaWorld.Payloads;

public record MovieDetails(
    string Title,
    string Year,
    string Rated,
    string Released,
    string Runtime,
    string Genre,
    string Director,
    string Writer,
    string Actors,
    string Plot,
    string Language,
    string Country,
    string Awards,
    string Poster,
    string Metascore,
    string Rating,
    string Votes,
    string ID,
    string Type,
    decimal Price
);