import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import MovieDetailsView, { MovieDetailsProps } from './MovieDetailsView';
import { MovieId, MovieDetails } from '@/app/lib/types';
import { fetchMovieDetails } from '@/app/lib/movieClient';

jest.mock('@/app/lib/movieClient', () => ({
    fetchMovieDetails: jest.fn(),
}));

describe('MovieDetailsView Component', () => {
    const queryClient = new QueryClient();

    beforeEach(() => {
        queryClient.clear();
        jest.clearAllMocks();
    });

    const mockMovieId: MovieId = {
        vendor: 'CinemaWorld',
        externalID: '1',
    };

    const mockMovieDetails: MovieDetails = {
        externalID: '1',
        title: 'Mock Movie',
        year: '2023',
        rated: 'PG-13',
        released: '2023-01-01',
        runtime: '120 min',
        genre: 'Action, Adventure',
        director: 'John Doe',
        writer: 'Jane Smith',
        actors: 'Actor A, Actor B',
        plot: 'This is a mock movie plot.',
        language: 'English',
        country: 'USA',
        awards: 'None',
        poster: 'https://example.com/mock-movie.jpg',
        metascore: '75',
        rating: '8.5',
        votes: '1000',
        type: 'movie',
        price: 19.99,
    };

    const defaultProps: MovieDetailsProps = {
        movieId: mockMovieId,
        onClose: jest.fn(),
    };

    it('renders a loading spinner while fetching movie details', async () => {
        (fetchMovieDetails as jest.Mock).mockResolvedValue(mockMovieDetails);

        render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...defaultProps} />
            </QueryClientProvider>
        );

        expect(screen.getByRole('spinner')).toBeInTheDocument();
    });

    it('renders movie details when data is fetched successfully', async () => {
        (fetchMovieDetails as jest.Mock).mockResolvedValue(mockMovieDetails);

        render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...defaultProps} />
            </QueryClientProvider>
        );

        await screen.findByText('Mock Movie');
        expect(screen.getByText('(2023)')).toBeInTheDocument();
        expect(screen.getByText('John Doe')).toBeInTheDocument();
    });

    it('renders a fallback message when movie details are null', async () => {
        (fetchMovieDetails as jest.Mock).mockResolvedValue(null);

        render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...defaultProps} />
            </QueryClientProvider>
        );

        await waitFor(() => {
            expect(screen.getByText('Movie details are not available.')).toBeInTheDocument();
        });
    });

    it('renders an error message when fetching movie details fails', async () => {
        (fetchMovieDetails as jest.Mock).mockRejectedValue(new Error('Failed to fetch movie details'));

        render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...defaultProps} />
            </QueryClientProvider>
        );

        waitFor(() => {
            expect(screen.getByText('Failed to load movie details.')).toBeInTheDocument();
        });
    });

    it('does not render when movieId is null', async () => {
        const propsWithNullMovieId: MovieDetailsProps = {
            ...defaultProps,
            movieId: null as unknown as MovieId,
        };

        const { container } = render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...propsWithNullMovieId} />
            </QueryClientProvider>
        );

        expect(container.firstChild).toBeNull();
    });

    it('calls onClose when the close button is clicked', async () => {
        (fetchMovieDetails as jest.Mock).mockResolvedValue(mockMovieDetails);

        render(
            <QueryClientProvider client={queryClient}>
                <MovieDetailsView {...defaultProps} />
            </QueryClientProvider>
        );

        fireEvent.click(await screen.getByRole('close'));
        expect(defaultProps.onClose).toHaveBeenCalled();
    });
});