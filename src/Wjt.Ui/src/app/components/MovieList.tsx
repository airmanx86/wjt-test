import { JSX } from "react";
import Image from "next/image";
import { Info, Flame } from "lucide-react";
import { MovieId, MovieItem, MoviePriceMap } from "@/app/lib/types";
import Spinner from "./Spinner";

export interface MovieListProps {
    movies: MovieItem[];
    moviePrices: MoviePriceMap;
    isLoading: boolean;
    isError: boolean;
    error: Error | null;
    searchTerm: string;
    onSelectMovie: (id: MovieId) => void;
    onHoverMovie: (id: MovieId) => void;
}

export default function MovieList({
    movies,
    moviePrices,
    isLoading,
    isError,
    error,
    searchTerm,
    onSelectMovie,
    onHoverMovie
}: MovieListProps): JSX.Element {
    const getPrice = (movie: MovieItem): number | undefined => {
        const movieKey = `${movie.vendor}-${movie.externalID}`;
        return movieKey in moviePrices ? moviePrices[movieKey].price : undefined;
    };

    const isCheapest = (movie: MovieItem): boolean => {
        const moviePrice = getPrice(movie);
        if (!moviePrice) {
            return false;
        }
        const cheapestPrice = Math.min(
            ...Object.values(moviePrices)
                .filter((price) => price.title === movie.title && price.year === movie.year)
                .map((price) => price.price)
        );
        return moviePrice === cheapestPrice;
    };

    if (isLoading) {
        return (
            <Spinner />
        );
    }

    if (isError) {
        return (
            <div className="text-center py-8">
                <p className="text-red-500 text-xl">Error: {error?.message || "Failed to fetch movies"}</p>
            </div>
        );
    }

    if (movies.length === 0 && searchTerm) {
        return (
            <div className="text-center py-12">
                <p className="text-xl text-gray-400">No movies found for {`"${searchTerm}"`}</p>
            </div>
        );
    }

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
            {movies.map((movie) => {
                const moviePrice = getPrice(movie);
                const isCheapestMovie = isCheapest(movie);

                return (
                <div
                    key={`${movie.vendor}-${movie.externalID}`}
                    className="bg-gray-800 rounded-lg overflow-hidden shadow-lg hover:shadow-xl transition-shadow duration-300 cursor-pointer"
                    onClick={() => onSelectMovie({ vendor: movie.vendor, externalID: movie.externalID })}
                    onMouseEnter={() => onHoverMovie({ vendor: movie.vendor, externalID: movie.externalID })}
                >
                    <Image
                        src={movie.poster}
                        alt={`${movie.title} poster`}
                        className="w-full h-64 object-scale-down"
                        width={300}
                        height={400}
                    />
                    <div className="p-4">
                        <h3 className="text-xl font-semibold mb-1">{movie.title}</h3>
                        <p className="text-gray-400">{movie.year}</p>
                        {moviePrice ? (
                            <p className="text-yellow-400 font-bold flex items-center justify-start">
                                <span>${moviePrice.toFixed(2)}</span>
                                {isCheapestMovie && <Flame className="h-4 w-4 text-red-500 ml-2" role="cheapest" />}
                            </p>
                        ) : (
                            <p className="text-gray-400">&nbsp;</p>
                        )}
                        <button
                            className="mt-2 flex items-center text-blue-400 hover:text-blue-300 transition-colors"
                        >
                            <Info className="h-4 w-4 mr-1" /> View details
                        </button>
                    </div>
                </div>
            )})}
        </div>
    );
}
