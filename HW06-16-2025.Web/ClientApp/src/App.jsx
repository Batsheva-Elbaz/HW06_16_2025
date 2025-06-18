import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

const App = () => {

    const [url, setUrl] = useState('');
    const [summary, setSummary] = useState('');
    const [loading, setLoading] = useState(false);

    const onButtonClick = async () => {
        setLoading(true);
        setSummary('');
        const response = await axios.post('/api/ai/generate', { url });
        setSummary(response.data.summary);
        setLoading(false);
    };

    return (
        <div className="container d-flex flex-column align-items-center justify-content-start min-vh-100" style={{ marginTop: 125 }}>
            <div className="form-group w-75 text-center">
                <h1>AI Article Summarizer</h1>
                <h4 className="form-lable">Paste artical URL:</h4>
                <input type="text" className="form-control" placeholder="http://example.com/news/artical" value={url} onChange={(e) => setUrl(e.target.value)}></input>
                <button onClick={onButtonClick} className="btn btn-info ">Summarize Artical</button>

                {loading && <p className="mt-3">Loading summary...</p>}
                {!loading && summary && (
                    <div className="form-group">
                        <h2>Summary:</h2>
                        <p>{summary}</p>
                    </div>
                )}
            </div>
        </div>

    );
};

export default App;