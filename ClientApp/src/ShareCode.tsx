import React, { useEffect, useState } from 'react';
import './style/ShareCode.css';

const InteractiveForm = ({sharecode}) => {

    function copyCodeButton() {

        let func = async () => {
            await navigator.clipboard.writeText(sharecode)
        }

        return <button className="sharecode-button" onClick={func}>Copy</button>
        
    }

    return (
        <div>
            <div className="sharecode-container">
                <div className="sharecode-title">Share: </div>
                <div className="sharecode-content">{sharecode}</div>
                {copyCodeButton()}
            </div>
        </div>

    );


}

export default InteractiveForm;