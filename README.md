# Weather Station with C# API and Vue.js Frontend

## Introduction

This project is a weather station application that uses a C# backend to interact with the Open Weather API and a Vue.js frontend to display weather data to users. The goal is to provide a user-friendly interface for accessing real-time weather information.

## Features

- **Real-Time Weather Data**: Fetch current weather data from the Open Weather API.
- **Vue.js Frontend**: A responsive and dynamic user interface for displaying weather information.
- **C# API Backend**: A robust backend service to handle API requests and data processing.

## Live Demo

Visit the live demo at: [www.ismail-mouyahada.com](http://www.ismail-mouyahada.com/)

## Technologies Used

- **Frontend**: Vue.js
- **Backend**: C#
- **API**: Open Weather API

## Installation and Setup

### Backend Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/Ismail-Mouyahada/Master-csharp-vuejs-app.git
   ```

2. Navigate to the backend project directory:

   ```bash
   cd Master-csharp-vuejs-app/backend
   ```

3. Restore the necessary packages:

   ```bash
   dotnet restore
   ```

4. Run the backend server:

   ```bash
   dotnet run
   ```

### Frontend Setup

1. Navigate to the frontend project directory:

   ```bash
   cd Master-csharp-vuejs-app/frontend
   ```

2. Install the required npm packages:

   ```bash
   npm install
   ```

3. Start the Vue.js development server:

   ```bash
   npm run serve
   ```

## Usage

1. Ensure that both the backend and frontend servers are running.
2. Open your browser and navigate to `http://localhost:8080` to access the Vue.js frontend.
3. Enter the desired location to fetch the current weather data.

## Project Structure

```plaintext
Master-csharp-vuejs-app/
├── backend/           # C# backend project
│   ├── Controllers/   # API controllers
│   ├── Models/        # Data models
│   ├── Services/      # Service classes
│   └── Program.cs     # Main entry point
└── frontend/          # Vue.js frontend project
    ├── src/
    │   ├── assets/    # Static assets
    │   ├── components/# Vue components
    │   ├── views/     # View files
    │   └── App.vue    # Main Vue component
    └── public/        # Public files
```

## Contributing

1. Fork the repository.
2. Create a new branch with your feature or bugfix.
3. Commit your changes.
4. Push the branch to your fork.
5. Create a pull request with a description of your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Contact

For any inquiries or feedback, please contact Ismail Mouyahada  

## Acknowledgements

- [Open Weather API](https://openweathermap.org/api)
- [Vue.js](https://vuejs.org/)
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

## Author

**Ismail.M**, Software Engineer & DevOps

- GitHub: [@ghostland-dev](https://github.com/ghostland-dev)
- Website: [ismail-mouyahada.com](https://www.ismail-mouyahada.com)
