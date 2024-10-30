import React from 'react';
import { createRoot } from 'react-dom/client';
import DateDisplay from './DateDisplay';
import ShareCode from './ShareCode';

const componentMap = {
    'DateDisplay': DateDisplay,
    'ShareCode': ShareCode,
};

document.addEventListener('DOMContentLoaded', () => {
    const elements: NodeListOf<HTMLElement> = document.querySelectorAll('div[data-react-component]');
    elements.forEach((element) => {
        const componentName = element.dataset.reactComponent;
        const Component = componentMap[componentName];
        if (Component) {
            const injectedData = element.getAttribute('data-args');
            const parsedData = JSON.parse(injectedData);
            const root = createRoot(element);
            root.render(<Component {...parsedData}/>);
        } else {
            console.error(`Component ${componentName} not found`);
        }
    });
});