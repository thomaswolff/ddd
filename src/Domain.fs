namespace Movies

module Domain =

    open System

    (*
        - Copyright owners
        - Movies
        - Cinema halls
        - Screenings
        - Cinemas
        - Cities
        - Customers
        - Tickets
    *)

    type MovieTitle = MovieTitle of string

    [<Measure>] type s
    [<Measure>] type min
    [<Measure>] type hour
    let secondsPerMinute = 60.<s/min>
    let minutesPerHour = 60.<min/hour>

    let convertMinutesToSeconds (minutes: float<min>) : float<s> =
        minutes * secondsPerMinute

    let convertHoursToMinutes (hours: float<hour>) : float<min> =
        hours * minutesPerHour

    let convertHoursToSeconds (hours: float<hour>) : float<s> =
        (convertHoursToMinutes hours) * secondsPerMinute

    type MovieDuration = private {
        Hours: float<hour>
        Minutes: float<min>
        Seconds: float<s>
    }

    module MovieDuration =

        let create (hours: float<hour>) (minutes: float<min>) (seconds: float<s>) : MovieDuration option =
            if (hours < 0.<hour> || minutes < 0.<min> || seconds < 0.<s>) then
                None
            else
                Some {
                    Hours = hours
                    Minutes = minutes
                    Seconds = seconds
                }

        let hours (movieDuration: MovieDuration) : float<hour> = movieDuration.Hours

        let minutes (movieDuration: MovieDuration) : float<min> = movieDuration.Minutes

        let seconds (movieDuration: MovieDuration) : float<s> = movieDuration.Seconds

        let totalSeconds (movieDuration: MovieDuration) : float<s> =
            [
                movieDuration.Seconds
                convertMinutesToSeconds movieDuration.Minutes
                convertHoursToSeconds movieDuration.Hours
            ]
            |> List.sum

    type MovieGenre =
        | Action
        | Comedy
        | Documentary
        | Drama
        | Horror
        | Thriller

    type Movie = {
        Title: MovieTitle
        Duration: MovieDuration
        Genre: MovieGenre
    }

    type CinemaName = CinemaName of string

    type SeatNumber = private SeatNumber of int

    module SeatNumber =
        let create (seatNumber: int) : SeatNumber option =
            if (seatNumber > 0) then
                Some <| SeatNumber seatNumber
            else
                None

        let value (SeatNumber seatNumber) = seatNumber

    type Seat = {
        Number: SeatNumber
    }

    type AscendingSortedSeats = private AscendingOrderedSeats of Seat list

    module AscendingOrderedSeats =

        let create (seats: Seat list) =
            seats
            |> List.sortBy (fun s -> s.Number)

        let value (AscendingOrderedSeats seats) = seats

    type RowNumber = private RowNumber of int

    module RowNumber =
        
        let create (rowNumber: int) : RowNumber option =
            if (rowNumber > 0) then
                Some <| RowNumber rowNumber
            else
                None

        let value (RowNumber rowNumber) = rowNumber

    type Row = {
        Number: RowNumber
        Seats: AscendingSortedSeats
    }

    type AscendingSortedRows = private AscendingSortedRows of Row list

    module AscendingSortedRows =

        let create (rows: Row list) : Row list =
            rows
            |> List.sortBy (fun r -> r.Number)

        let value (AscendingSortedRows rows) = rows

    type CinemaHall = {
        Rows: AscendingSortedRows
    }

    type Screening = private {
        Movie: Movie
        From: DateTimeOffset
        To: DateTimeOffset
    }

    module Screening =

        let private maxCommercialDuration = 15.<min>
        
        let create (movie: Movie) (fromTime: DateTimeOffset) (toTime: DateTimeOffset) : Screening option =
            let screeningDuration : float<s> = LanguagePrimitives.FloatWithMeasure (toTime - fromTime).TotalSeconds
            let movieDuration = MovieDuration.totalSeconds movie.Duration

            if (screeningDuration < movieDuration) then
                None
            else if (screeningDuration > movieDuration + convertMinutesToSeconds maxCommercialDuration) then
                None
            else 
                Some {
                    Screening.Movie = movie
                    From = fromTime
                    To = toTime
                }

    type Cinema = {
        Name: CinemaName
        Halls: CinemaHall list
    }

    type MovieRights = {
        Movie: Movie
        Cinema: Cinema
        From: DateTimeOffset
        To: DateTimeOffset
    }
