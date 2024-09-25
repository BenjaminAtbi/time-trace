import React, { useEffect, useState } from 'react';
import './style/DateDisplay.css';
import dayjs from './dayjs';
var sprintf = require('sprintf-js').sprintf;



const InteractiveForm = ({user, initialUserTimes, initialOtherTimes}) => {

    type CellCallback = (date: dayjs.Dayjs) => void;

    const [baseDate, setBaseDate] = useState<dayjs.Dayjs>(dayjs().startOf('day'));
    const [userTimeSlots, setUserTimeSlots] = useState<Map<number, Set<string>>>(ingestUserTimes(initialUserTimes));
    const [otherTimeSlots, setOtherTimeSlots] = useState<Map<number, Set<string>>>(ingestOtherTimes(initialOtherTimes));

    function ingestUserTimes(initialUserTimes): Map<number, Set<string>> {
        if (!Array.isArray(initialUserTimes)) throw Error("initialUserTimes is not array");
        let injestedTimes = new Map<number, Set<string>>();
        initialUserTimes.forEach((d) => {
            if (!dayjs(d).isValid()) throw Error('date in initialUserTimes invalid: ' + String(d));
            injestedTimes.set(dayjs(d).startOf('hour').valueOf(), new Set<string>(user));
        });
        return injestedTimes;
    }

    function ingestOtherTimes(initialOtherTimes): Map<number, Set<string>> {
        return new Map<number, Set<string>>();
    }

    function userCellOnclick(date: dayjs.Dayjs) {


        //ajax
    }


    //function genDateCells(hour: number): Array<JSX.Element> {
    //    return Array.from(Array(7).keys(), day => {
    //        let refDate = baseDate.add(day, 'day').hour(hour);
    //        return <td
    //            className={'timeSlot ' + (userTimeSlots.has(refDate.valueOf()) ? 'selectedTime' : 'deselectedTime') }
    //            key={refDate.valueOf()}
    //        ></td>
    //    });
    //}

    function genTimeSlotGrid(refSlots: Map<number, Set<string>>, callback: CellCallback) {
        return Array.from(Array(24).keys(), h => {
            let hour = h + 1;
            console.log(refSlots);
            return <tr className='timeRow' key={hour}>
                <td className="timeSlot">
                    <p className="timeRowHeader">
                        {hour > 12 ? sprintf('%d pm', hour - 12) : sprintf('%d am', hour)}
                    </p>
                </td>
                {   Array.from(Array(7).keys(), day => {
                        let refDate = baseDate.add(day, 'day').hour(hour);
                        return <td
                            className={'timeSlot ' + (refSlots.has(refDate.valueOf()) ? 'selectedTime' : 'deselectedTime')}
                            key={refDate.valueOf()}
                        ></td>
                    })
                }
            </tr>
        });
    }

    const dateHeaderCells: Array<JSX.Element> = Array.from(Array(7).keys(), n =>
        <td className='timeSlot' key={n}>
            <p className='timeSlotHeader'> {baseDate.add(n, 'day').format('dddd, MMM D')}</p>
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
                        {genTimeSlotGrid(userTimeSlots, null)}
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
                        {genTimeSlotGrid(otherTimeSlots, null)}
                    </tbody>
                </table>
            </div>
        </div >
    );
};

export default InteractiveForm;