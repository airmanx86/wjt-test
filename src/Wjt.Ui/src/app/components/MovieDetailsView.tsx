import { JSX } from "react";
import { X, Clock, Star, CircleCheckBig, CalendarCheck2 } from "lucide-react";
import { useQuery } from '@tanstack/react-query';
import { MovieId, MovieDetails } from "@/app/lib/types";
import { fetchMovieDetails } from '@/app/lib/movieClient';
import Spinner from "./Spinner";

export interface MovieDetailsProps {
    movieId: MovieId;
    onClose: () => void;
}

export default function MovieDetailsView({ movieId, onClose }: MovieDetailsProps): JSX.Element | null {
    const { data: movieDetails, isLoading, isError } = useQuery<MovieDetails, Error>({
        queryKey: ['movie', movieId],
        queryFn: () => fetchMovieDetails(movieId),
        enabled: movieId !== null,
    });

    if (movieId === null) {
        return null;
    }

    return (
        <div className="fixed inset-0 bg-black bg-opacity-75 flex items-center justify-center p-4 z-50">
            <div className="bg-gray-800 rounded-lg max-w-2xl w-full max-h-screen overflow-y-auto">
                <div className="relative">
                    <button
                        role="close"
                        onClick={onClose}
                        className="absolute top-2 right-2 bg-gray-700 hover:bg-gray-600 rounded-full p-2 text-white z-10"
                    >
                        <X className="h-6 w-6" />
                    </button>

                    {isLoading && (
                        <div className="mt-6">
                            <Spinner />
                        </div>
                    )}

                    {!isLoading && movieDetails && (
                        <div>
                            <div className="flex flex-col md:flex-row p-6 mb-6 bg-gray-900 rounded-t-lg">
                                <img
                                    src={movieDetails.poster}
                                    alt={`${movieDetails.title} poster`}
                                    className="w-full md:w-64 h-auto object-cover rounded-t-lg md:rounded-l-lg md:rounded-t-none"
                                />
                                <div className="p-6 flex flex-col">
                                    <h2 className="text-2xl font-bold mb-2">{movieDetails.title} <span className="text-gray-400">({movieDetails.year})</span></h2>

                                    <div className="flex items-center mb-4">
                                        <Star className="h-5 w-5 text-yellow-400 mr-1" />
                                        <span className="font-semibold">{movieDetails.rating}/10</span>
                                        <CircleCheckBig className="h-5 w-5 text-green-400 ml-2" />
                                        <span className="text-gray-400 ml-2">{movieDetails.votes}</span>
                                        <span className="bg-gray-700 text-white px-2 py-1 rounded text-sm ml-2">{movieDetails.metascore}</span><span className="text-gray-400 text-xs ml-2">Metascore</span>
                                    </div>

                                    <div className="flex flex-wrap mb-2">
                                        {movieDetails.genre.split(', ').map((genre, index) => (
                                            <span key={index} className="bg-gray-700 px-2 py-1 rounded text-sm mr-2 mb-2">
                                                {genre}
                                            </span>
                                        ))}
                                    </div>

                                    <div className="flex items-center text-gray-300 mb-2">
                                        <Clock className="h-4 w-4 mr-2" />
                                        <span>{movieDetails.runtime}</span>
                                    </div>

                                    <div className="flex items-center text-gray-300 mb-4">
                                        <CalendarCheck2 className="h-4 w-4 mr-2" />
                                        <span className="font-medium">{movieDetails.released}</span>
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Director:</span> {movieDetails.director}
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Writer:</span> {movieDetails.writer}
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Actors:</span> {movieDetails.actors}
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Language:</span> {movieDetails.language}
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Country:</span> {movieDetails.country}
                                    </div>

                                    <div className="text-gray-300 mb-4">
                                        <span className="font-medium">Awards:</span> {movieDetails.awards || 'N/A'}
                                    </div>

                                    <div className="text-yellow-400 font-bold text-lg mb-4">
                                        <span>Price:</span> ${movieDetails.price.toFixed(2)}
                                    </div>
                                </div>
                            </div>
                            <div className="p-6 pt-0">
                                <h3 className="text-xl font-semibold mb-2">Overview</h3>
                                <p className="text-gray-300">{movieDetails.plot}</p>
                            </div>
                        </div>
                    )}

                    {!isLoading && !movieDetails && (
                        <div className="p-6">
                            <p className="text-red-500">Movie details are not available.</p>
                        </div>
                    )}

                    {isError && (
                        <div className="p-6">
                            <p className="text-red-500">Failed to load movie details.</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}