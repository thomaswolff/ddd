# DDD

Repo for exploring ideas related to Domain Driven Design

## Domain that I'm modelling

Movie theaters

What does the domain consist of?

- Copyright owners
- Movies
- Cinema halls
- Screenings
- Cinemas
- Cities
- Customers
- Tickets

What processes are a part of the domain?

- Acquire rights for movie
- Order ticket to screening
- Register ticket at screening
- Movie screening

What bounded contexts exists in the domain?

- Rights
- Administratoren
- Analytics
- Logging
- Ticketing
- Screening

How should the bounded contexts be represented by systems?

- Implementation detail
- For the time being we can just model our domain and worry about mappings from bounded contexts to systems later

## Ideas I want to explore

- "Keep IO at the edges"
  - Why?
    - So that the core of the domain can be pure
- "Make invalid state unreprecentable"
  - Why?
    - Fewer bugs
- Pure functions in core of application
  - Motivation
    - No side effects
    - Makes code easier to reason about
    - The code is deterministic
    - Easier to test
- Error handling
  - Option
    - Most suited for required/not required?
  - Result
    - Most suited for modelling errors that can happen as a part of our domain?
  - Exception
    - Most suited for error that can occur, but not a part of our domain? Infrastructure, runtime etc.

## Implementation

### Top level process

```txt
------ IO start ------
- Get some input
  - HTTP request
  - Queue
  - User event in native application
- Deserialize input to DTO
  - Simple types that correspond to the format the data is being transmitted in
  - This data can be corrupt
- Map DTO to domain
  - Rely on a domain model that can only represent valid states in your domain
------ IO end ------

------ Pure functions start ------
- Perform some operation in the domain
  - Given a valid state in our domain, perform an operation/transformation on it
  - Domain model should reflect the mental model (ubiquitous language/shared model) so that everyone involved in the project (developers, architects, designers, stake holders, domain experts) understand each other, and can communicate about the domain. In addition, the code becomes a documentation of the domain.
------ Pure functions end ------

------ IO start ------
- When the operation in the domain is completed, we probably want this to make a change outside of our program
  - HTTP response
  - Record updates in a database
  - Add a message to a queue
- Serialize the output of our operation to a DTO
------ IO end ------
```

The program can consist of multiple IO/pure/IO-layers

### Domain model

- Should contain the core data structures, their rules and legal operations
- Is defined in `Domain.fs`
- Should not know of DTOs or IO
