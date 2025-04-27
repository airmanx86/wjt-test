import config from './config';
import { MovieItem, MovieDetails, MovieId } from './types';

export async function fetchMovies(searchTerm: string): Promise<MovieItem[]> {
    const response = await fetch(`${config.moviesApiBaseUrl}/api/movies?title=${searchTerm}`);

    if (!response.ok) {
        throw new Error('Failed to fetch movies');
    }

    return response.json();
}

export async function fetchMovieDetails(movieId: MovieId): Promise<MovieDetails> {
    const response = await fetch(`${config.moviesApiBaseUrl}/api/movies/${encodeURI(movieId.vendor)}/${encodeURI(movieId.externalID)}`);

    if (!response.ok) {
        throw new Error('Failed to fetch movies');
    }

    return response.json();
}