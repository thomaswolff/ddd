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

    // Wrong use of units of measure?
    [<Measure>] type s
    [<Measure>] type min
    [<Measure>] type hour
    let secondsPerMinute = 60<s/min>
    let minutesPerHour = 60<min/hour>

    type MovieDuration = {
        Hours: int<hour>
        Minutes: int<min>
        Seconds: int<s>
    }

    module MovieDuration =

        let private minutesToSeconds (minutes: int<min>) : int<s> =
            minutes * secondsPerMinute

        let private hoursToMinutes (hours: int<hour>) : int<min> =
            hours * minutesPerHour

        let private hoursToSeconds (hours: int<hour>) : int<s> =
            (hoursToMinutes hours) * secondsPerMinute

        let totalSeconds (movieDuration: MovieDuration) : int<s> =


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

    type RowNumber = private RowNumber of int

    module RowNumber =
        
        let create (rowNumber: int) : RowNumber option =
            if (rowNumber > 0) then
                Some <| RowNumber rowNumber
            else
                None

        let value (RowNumber rowNumber) = rowNumber

    type Row = {
        RowNumber: RowNumber
        Seat list
    }

    type CinemaHall = {
        Rows: Row list
    }

    type Screening = private {
        Movie: Movie
        From: DateTimeOffset
        To: DateTimeOffset
    }

    module Screening =
        
        let create (movie: Movie) (fromTime: DateTimeOffset) (toTime: DateTimeOffset) : Screening option =
            let screeningDurationInSeconds = (toTime - fromTime).TotalSeconds
            if (screeningDurationInSeconds < 0.) then
                None
            else if (screeningDurationInSeconds = movie.)
            else
                None

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
