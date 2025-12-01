import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Forecast } from './components/Forecast';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Aurora Forecast</h1>
      </header>
      <main>
        <Forecast />
      </main>
    </div>
  );
}

export default App;
