import React, { useEffect, useState } from 'react';
import './style/DateDisplay.css';
import addDays from './lib';
var sprintf = require('sprintf-js').sprintf;

const InteractiveForm = ({initialdata}) => {

    const [baseDate, setBaseDate] = useState<Date>(new Date(new Date().toDateString()));
    const [userTimeSlots, setUserTimeSlots] = useState<Set<Date>>();
    const [otherTimeSlots, setOtherTimeSlots] = useState<Map<string, Set<Date>>>();

    useEffect(() => {
        Initialize();
    }, []);

    function Initialize() {

    }


    //const handleInputChange = (e) => {
    //    const { name, value } = e.target;
    //    setFormData(prevState => ({
    //        ...prevState,
    //        [name]: value
    //    }));
    //};

    //const setDefaultValues = () => {
    //    setFormData({ name: 'John Doe', email: 'john@example.com' });
    //};

    //const handleSubmit = (e) => {
    //    e.preventDefault();
    //    // Here you would typically send the data to your server
    //    console.log('Form submitted:', formData);
    //};


    function genDateCells(hour: number): Array<JSX.Element> {
        return Array.from(Array(7).keys(), n =>
            <td className='timeSlot'>
                
            </td>
        );
    }

    const timeRows: Array<JSX.Element> = Array.from(Array(24).keys(), n => {
        let hour = n + 1;
        return <tr className='timeRow'>
            <td className="timeSlot">
                <p className="timeRowHeader">
                    {hour > 12 ? sprintf('%d pm', hour - 12) : sprintf('%d am', hour)}
                </p>
            </td>
            {genDateCells(hour)}
        </tr>
    });

    const dateHeaderCells: Array<JSX.Element> = Array.from(Array(7).keys(), n =>
        <td className='timeSlot'>
            <p className='timeSlotHeader'> {addDays(baseDate, n).toDateString().slice(0, length - 4)}</p>
        </td>
    ); 

    return (
        <div className="dateDisplayContainer">
            <div className="dateTableWrapper">
                <h3>Your Times</h3>
                <table className="dateTimeTable">
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr>
                    </thead>
                    <tbody>
                        {timeRows}
                    </tbody>
                </table>
            </div >
            <div className="dateTableWrapper">
                {/*<h3>Other Users: @otherDateTimes.Keys.Aggregate("", (s, next) => $"{s} {next}")</h3>*/}
                <table className="dateTimeTable">
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr>
                    </thead>
                    <tbody>
                        {timeRows}
                    </tbody>
                </table>
            </div>
        </div >
    );
};

export default InteractiveForm;