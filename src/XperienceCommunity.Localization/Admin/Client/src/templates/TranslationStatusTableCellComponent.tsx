import Tooltip from '@mui/material/Tooltip';
import React from 'react';
import { MdCheckCircle, MdError, MdWarning } from 'react-icons/md';

export type TranslationStatus = 'complete' | 'partial' | 'none';

export interface TranslationStatusTableCellComponentProps {
    status: TranslationStatus;
    missingLanguageNames: string[];
}

const statusColorByStatus: Record<TranslationStatus, string> = {
    complete: '#2e7d32',
    partial: '#ed6c02',
    none: '#d32f2f',
};

const StatusIcon = ({ status }: { status: TranslationStatus }): JSX.Element => {
    const color = statusColorByStatus[status];

    if (status === 'complete') {
        return <MdCheckCircle color={color} size={18} />;
    }

    if (status === 'partial') {
        return <MdWarning color={color} size={18} />;
    }

    return <MdError color={color} size={18} />;
};

const getTooltipText = (props: TranslationStatusTableCellComponentProps): string => {
    if (props.status === 'complete') {
        return 'All languages translated';
    }

    if (props.status === 'none') {
        return 'No translations';
    }

    return `Missing: ${props.missingLanguageNames.join(', ')}`;
};

export const TranslationStatusTableCellComponent = (
    props: TranslationStatusTableCellComponentProps,
): JSX.Element => {
    return (
        <Tooltip title={getTooltipText(props)}>
            <span style={{ display: 'inline-flex', alignItems: 'center' }}>
                <StatusIcon status={props.status} />
            </span>
        </Tooltip>
    );
};
