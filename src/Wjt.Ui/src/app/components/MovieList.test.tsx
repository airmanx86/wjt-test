import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import MovieList, { MovieListProps } from './MovieList';
import { MovieItem } from '@/app/lib/types';

describe('MovieList Component', () => {
    const mockMovies: MovieItem[] = [
        {
            externalID: '1',
            title: 'Movie 1',
            year: '2021',
            poster: 'https://example.com/movie1.jpg',
            vendor: 'CinemaWorld',
        },
        {
            externalID: '2',
            title: 'Movie 2',
            year: '2022',
            poster: 'https://example.com/movie2.jpg',
            vendor: 'FilmWorld',
        },
    ];

    const defaultProps: MovieListProps = {
        movies: mockMovies,
        isLoading: false,
        isError: false,
        error: null,
        searchTerm: '',
        onSelectMovie: jest.fn(),
        onHoverMovie: jest.fn(),
    };

    it('renders a loading spinner when isLoading is true', async () => {
        render(<MovieList {...defaultProps} isLoading={true} />);
        expect(screen.getByRole('spinner')).toBeInTheDocument();
    });

    it('renders an error message when isError is true', () => {
        const errorMessage = 'Failed to fetch movies';
        render(<MovieList {...defaultProps} isError={true} error={new Error(errorMessage)} />);
        expect(screen.getByText(`Error: ${errorMessage}`)).toBeInTheDocument();
    });

    it('renders a message when no movies are found for the search term', () => {
        render(<MovieList {...defaultProps} movies={[]} searchTerm='Test' />);
        expect(screen.getByText('No movies found for "Test"')).toBeInTheDocument();
    });

    it('renders a list of movies', () => {
        render(<MovieList {...defaultProps} />);
        expect(screen.getByText('Movie 1')).toBeInTheDocument();
        expect(screen.getByText('Movie 2')).toBeInTheDocument();
    });

    it('calls onSelectMovie when a movie is clicked', () => {
        const onSelectMovie = jest.fn();
        render(<MovieList {...defaultProps} onSelectMovie={onSelectMovie} />);

        fireEvent.click(screen.getByText('Movie 1'));
        expect(onSelectMovie).toHaveBeenCalledWith({ vendor: 'CinemaWorld', externalID: '1' });
    });

    it('calls onHoverMovie when a movie is hovered', () => {
        const onHoverMovie = jest.fn();
        render(<MovieList {...defaultProps} onHoverMovie={onHoverMovie} />);

        fireEvent.mouseEnter(screen.getByText('Movie 1'));
        expect(onHoverMovie).toHaveBeenCalledWith({ vendor: 'CinemaWorld', externalID: '1' });
    });
});