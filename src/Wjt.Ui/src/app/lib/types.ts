export type MovieVendor = 'CinemaWorld' | 'FilmWorld';

export type MovieId = {
    vendor: MovieVendor;
    externalID: string;
}

export interface MovieItem {
    externalID: string;
    title: string;
    year: string;
    poster: string;
    vendor: MovieVendor;
}

export interface MovieDetails {
    externalID: string;
    title: string;
    year: string;
    rated: string;
    released: string;
    runtime: string;
    genre: string;
    director: string;
    writer: string;
    actors: string;
    plot: string;
    language: string;
    country: string;
    awards?: string;
    poster: string;
    metascore: string;
    rating: string;
    votes: string;
    type: string;
    price: number;
}

export type MoviePriceMap = {
    [key: string]: {
        title: string;
        year: string;
        price: number;
    }
};