'use client';

import { JSX, useState, useEffect } from 'react';
import { Search } from 'lucide-react';
import { useQuery, QueryClient, QueryClientProvider, useQueryClient } from '@tanstack/react-query';
import { MovieItem, MovieId, MoviePriceMap } from '@/app/lib/types';
import { fetchMovies, fetchMovieDetails } from '@/app/lib/movieClient';
import MovieList from '@/app/components/MovieList';
import MovieDetailsView from '@/app/components/MovieDetailsView';

function MoviePriceCheckApp(): JSX.Element {
    const [queryClient] = useState(() => new QueryClient());

    return (
        <QueryClientProvider client={queryClient}>
            <MovieSearch />
        </QueryClientProvider>
    );
}

function MovieSearch(): JSX.Element {
    const [searchQuery, setSearchQuery] = useState('');
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedMovieId, setSelectedMovieId] = useState<MovieId | null>(null);
    const [moviePrices, setMoviePrices] = useState<MoviePriceMap>({});
    const queryClient = useQueryClient();

    const { data: movies = [], isLoading, isError, error } = useQuery<MovieItem[], Error>({
        queryKey: ['movies', searchTerm],
        queryFn: () => fetchMovies(searchTerm),
        enabled: !!searchTerm,
        select: (data) => {
            return data.sort((a, b) => {
                return a.title.localeCompare(b.title);
            });
        }
    });

    const prefetchMovieDetails = (movieId: MovieId): void => {
        queryClient.prefetchQuery({
          queryKey: ['movie', movieId],
          queryFn: async () => { 
            const details = await fetchMovieDetails(movieId);

            if (details) {
                setMoviePrices((prevPrices) => ({
                    ...prevPrices,
                    [`${movieId.vendor}-${movieId.externalID}`]: {
                        title: details.title,
                        year: details.year,
                        price: details.price,
                    }
                }));
            }

            return details;
          },
          staleTime: 1 * 60 * 1000,
        });
    };

    const handleSearch = (evt: React.FormEvent<HTMLFormElement>) => {
        evt.preventDefault();
        setSearchTerm(searchQuery.trim());
    };

    useEffect(() => {
        if (movies.length > 0) {
            movies.forEach((movie) => {
                prefetchMovieDetails({ vendor: movie.vendor, externalID: movie.externalID });
            });
        }
    }, [movies]);

    return (
        <div className="flex flex-col items-center min-h-screen bg-gray-900 text-white py-8 px-4">
            <div className="w-full max-w-4xl">
                <h1 className="text-3xl font-bold mb-8 text-center">Movie Price Check</h1>

                <form onSubmit={handleSearch} className="mb-8">
                    <div className="relative">
                        <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
                            <Search className="h-5 w-5 text-gray-400" />
                        </div>
                        <input
                            type="text"
                            className="w-full p-4 pl-10 text-lg bg-gray-800 border border-gray-700 rounded-lg focus:ring-blue-500 focus:border-blue-500"
                            placeholder="Search for movies..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                        <button
                            type="submit"
                            className="absolute right-2.5 bottom-2.5 bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-800 font-medium rounded-lg text-sm px-4 py-2"
                        >
                            Search
                        </button>
                    </div>
                </form>

                <MovieList
                    movies={movies}
                    moviePrices={moviePrices}
                    isLoading={isLoading}
                    isError={isError}
                    error={error}
                    searchTerm={searchTerm}
                    onSelectMovie={(movieId) => setSelectedMovieId(movieId)}
                    onHoverMovie={(movieId) => prefetchMovieDetails(movieId)}
                />

                <MovieDetailsView
                    movieId={selectedMovieId!}
                    onClose={() => setSelectedMovieId(null)}
                />
            </div>
        </div>
    );
}

export default MoviePriceCheckApp;