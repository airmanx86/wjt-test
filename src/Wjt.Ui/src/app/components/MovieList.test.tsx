import { render, screen, fireEvent } from '@testing-library/react';
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
            externalID: '1',
            title: 'Movie 1',
            year: '2021',
            poster: 'https://example.com/movie1.jpg',
            vendor: 'FilmWorld',
        },
        {
            externalID: '2',
            title: 'Movie 2',
            year: '2022',
            poster: 'https://example.com/movie2.jpg',
            vendor: 'FilmWorld',
        },

    ];

    const mockMoviePrices = {
        'CinemaWorld-1': { title: 'Movie 1', year: '2021', price: 10 },
        'FilmWorld-1': { title: 'Movie 1', year: '2022', price: 9 },
        'FilmWorld-2': { title: 'Movie 2', year: '2022', price: 15 },
    };

    const defaultProps: MovieListProps = {
        movies: mockMovies,
        moviePrices: mockMoviePrices,
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
        const movie1 = screen.getAllByText('Movie 1');
        expect(movie1.length).toBe(2);
        expect(screen.getByText('Movie 2')).toBeInTheDocument();
    });

    it('calls onSelectMovie when a movie is clicked', () => {
        const onSelectMovie = jest.fn();
        render(<MovieList {...defaultProps} onSelectMovie={onSelectMovie} />);

        fireEvent.click(screen.getByText('Movie 2'));
        expect(onSelectMovie).toHaveBeenCalledWith({ vendor: 'FilmWorld', externalID: '2' });
    });

    it('calls onHoverMovie when a movie is hovered', () => {
        const onHoverMovie = jest.fn();
        render(<MovieList {...defaultProps} onHoverMovie={onHoverMovie} />);

        fireEvent.mouseEnter(screen.getByText('Movie 2'));
        expect(onHoverMovie).toHaveBeenCalledWith({ vendor: 'FilmWorld', externalID: '2' });
    });

    it('renders the price of each movie if available', () => {
        render(<MovieList {...defaultProps} />);
        expect(screen.getByText('$10.00')).toBeInTheDocument();
        expect(screen.getByText('$9.00')).toBeInTheDocument();
        expect(screen.getByText('$15.00')).toBeInTheDocument();
    });

    it('does not render a price if it is not available', () => {
        render(<MovieList {...defaultProps} moviePrices={{}} />);
        expect(screen.queryByText('$')).not.toBeInTheDocument();
    });

    it('renders a fire icon next to the price if the movie is the cheapest', () => {
        render(<MovieList {...defaultProps} />);
        const fireIcon = screen.getAllByRole('cheapest', { hidden: true });
        expect(fireIcon.length).toBe(2);
    });
});