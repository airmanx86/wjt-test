namespace Wjt.Movies.Payloads;

public record MovieItem(
    string Title,
    string Year,
    string ExternalID,
    string Poster,
    string Vendor
);